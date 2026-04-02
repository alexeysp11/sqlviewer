using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SqlViewer.Shared.Converters;

public sealed class DynamicObjectConverter : JsonConverter<object>
{
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number => reader.TryGetInt64(out long l) ? l : reader.GetDouble(),
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.StartObject => ReadObject(ref reader),
            JsonTokenType.StartArray => ReadList(ref reader),
            _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
        };
    }

    private object ReadObject(ref Utf8JsonReader reader)
    {
        IDictionary<string, object?> expando = new ExpandoObject();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) return expando;
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string? propertyName = reader.GetString();

                if (string.IsNullOrEmpty(propertyName))
                    continue;

                reader.Read();
                expando[propertyName] = Read(ref reader, typeof(object), null!);
            }
        }
        return expando;
    }

    private List<object?> ReadList(ref Utf8JsonReader reader)
    {
        List<object?> list = [];
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                return list;
            object? value = Read(ref reader, typeof(object), null!);
            list.Add(value);
        }
        return list;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
