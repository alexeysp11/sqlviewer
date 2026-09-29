# Архитектурное расширение: Внедрение Elasticsearch как Read-Model для глобального поиска по метаданным в sqlviewer

## 🎯 Постановка задачи (Problem Statement)
Система `sqlviewer` предназначена для управления гетерогенными (разнородными) источниками данных. По мере масштабирования платформы и подключения десятков различных СУБД (PostgreSQL, MS SQL, MySQL, Oracle) прямые поисковые запросы к транзакционным базам данных (OLTP) для поиска метаданных (названий таблиц, схем, типов полей, комментариев) становятся неэффективными. 

Сканирование системных каталогов через тяжелые SQL-операторы `LIKE '%text%'` приводит к деградации производительности целевых СУБД. Кроме того, реляционные базы не поддерживают нечеткий поиск (с учетом опечаток), фасетную фильтрацию и мгновенный полнотекстовый поиск по фрагментам кода или логов.

## 🏗️ Архитектурное решение: Паттерн CQRS & Read-Model в Elasticsearch

Для изоляции поисковой нагрузки и обеспечения наносекундного отклика системы внедряется паттерн **CQRS (Command Query Responsibility Segregation)**. 
* **Слой записи (Command):** Изменения в структурах баз данных фиксируются целевыми воркерами и отправляются в шину **Apache Kafka**.
* **Слой чтения (Query):** Elasticsearch выступает в роли единого сквозного поискового индекса — денормализованной **Read-Model**.

### Схема интеграции данных:
```text
[Гетерогенные СУБД] ──(CDC / Kafka)──> [Data Sync Worker] ──> [Elasticsearch Index]
                                                                        ▲
[User API Client] ──────────────────────────────────────────────────────┘ (Глобальный поиск)
```

1. При изменении схемы данных (создание таблицы, изменение типа колонки) публикуется событие в Kafka.
2. Специализированный микросервис **Data Sync Worker** вычитывает события из Kafka, схлопывает (денормализует) метаданные в плоский JSON-документ и выполняет атомарный `Upsert` в индекс Elasticsearch.
3. Пользовательский API делает поисковые запросы **исключительно к API Elasticsearch**, минуя живые реляционные базы данных.

---

## 🗂️ 1. Спецификация Поискового Индекса (Elasticsearch Mapping)

Индекс `sqlviewer-metadata` оптимизирован для полнотекстового поиска с использованием n-граммных анализаторов и нечеткой логики (Fuzzy Search).

```json
{
  "settings": {
    "analysis": {
      "analyzer": {
        "snake_case_analyzer": {
          "type": "custom",
          "tokenizer": "underscore_tokenizer",
          "filter": ["lowercase"]
        }
      },
      "tokenizer": {
        "underscore_tokenizer": {
          "type": "pattern",
          "pattern": "_"
        }
      }
    }
  },
  "mappings": {
    "properties": {
      "Id": { "type": "keyword" },
      "TargetDatabaseId": { "type": "keyword" },
      "DatabaseType": { "type": "keyword" }, // PostgreSQL, MSSQL, Oracle, etc.
      "SchemaName": { "type": "keyword" },
      "TableName": {
        "type": "text",
        "analyzer": "snake_case_analyzer",
        "fields": {
          "keyword": { "type": "keyword" }
        }
      },
      "Columns": {
        "type": "nested",
        "properties": {
          "Name": {
            "type": "text",
            "fields": { "keyword": { "type": "keyword" } }
          },
          "DataType": { "type": "keyword" },
          "IsNullable": { "type": "boolean" },
          "Comment": { "type": "text" }
        }
      },
      "Description": { "type": "text" },
      "UpdatedAt": { "type": "date" }
    }
  }
}
```

---

## 🛠️ 2. Реализация в .NET Core 9 (Слой Интеграции)

Внедрение официального клиента `Elastic.Clients.Elasticsearch` (для .NET 9) с конфигурацией Fluent Mapping.

### Доменная модель Read-Model (Shared / Contracts):
```csharp
public sealed record TableMetadataDocument
{
    public string Id { get; init; } = string.Empty; // DatabaseId_Schema_Table
    public string TargetDatabaseId { get; init; } = string.Empty;
    public string DatabaseType { get; init; } = string.Empty;
    public string SchemaName { get; init; } = string.Empty;
    public string TableName { get; init; } = string.Empty;
    public List<ColumnMetadataDto> Columns { get; init; } = [];
    public string? Description { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public sealed record ColumnMetadataDto(
    string Name, 
    string DataType, 
    bool IsNullable, 
    string? Comment
);
```

### Поисковый сервис (Search API Service):
```csharp
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

public sealed class GlobalSearchService : IGlobalSearchService
{
    private readonly ElasticsearchClient _elasticClient;

    public GlobalSearchService(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<List<TableMetadataDocument>> SearchMetadataAsync(string userQuery, string? dbTypeFilter = null)
    {
        var response = await _elasticClient.SearchAsync<TableMetadataDocument>(s => s
            .Index("sqlviewer-metadata")
            .Query(q => q
                .Bool(b => b
                    .Should(
                        sh => sh.Match(m => m.Field(f => f.TableName).Query(userQuery).Fuzziness(new Fuzziness("AUTO"))),
                        sh => sh.Nested(n => n
                            .Path(p => p.Columns)
                            .Query(nq => nq.Match(m => m.Field("Columns.Name").Query(userQuery)))
                        )
                    )
                    .Filter(f => 
                        !string.IsNullOrEmpty(dbTypeFilter) 
                            ? [new TermQuery(new Field("DatabaseType")) { Value = dbTypeFilter }] 
                            : Array.Empty<Query>()
                    )
                )
            )
        );

        if (!response.IsValidResponse)
        {
            throw new InvalidOperationException($"Elasticsearch query failed: {response.DebugInformation}");
        }

        return response.Documents.ToList();
    }
}
```

---

## ⚡ 3. Фичи, которые приобретает `sqlviewer`:

1. **Нечеткий глобальный поиск (Fuzzy Search Layer):** Пользователь может ввести в поисковую строку `uzer_profels`, и система мгновенно выдаст таблицу `user_profiles` из PostgreSQL за счет встроенного расстояния Левенштейна в Elastic.
2. **Фасетные агрегации (Faceting):** За один единственный запрос API возвращает не только список совпадений, но и счетчики по типам СУБД: *«Найдено 45 таблиц: 30 в СУБД PostgreSQL, 15 в MS SQL»*.
3. **Изоляция OLTP-нагрузки:** Любой поисковый хаос на UI больше не генерирует ни одного `SELECT`-запроса к реальным базам данных пользователей.
