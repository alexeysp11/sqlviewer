CREATE TABLE IF NOT EXISTS translation 
(
    translation_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    context VARCHAR(75) NOT NULL, -- Name of UI element or module 
    english TEXT NOT NULL, 
    german TEXT, 
    russian TEXT, 
    spanish TEXT, 
    portugues TEXT, 
    italian TEXT, 
    french TEXT, 
    ukranian TEXT, 
    dutch TEXT
); 

INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'File', 'Datei', 'Файл', 'Expediente', 'Arquivo', 'File', 'Fichier', 'Файл', 'Het dossier'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'New', 'Schaffen', 'Создать', 'crear', 'crio', 'creare', 'créer', 'створювати', 'creëren'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Open', 'offen', 'Открыть', 'abrir', 'abrir', 'aprire', 'ouvrir', 'відчинено', 'Open'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Reopen', 'wieder öffnen', 'Открыть снова', 'reabrir', 'reabrir', 'riaprire', 'rouvrir', 'знову відкрити', 'heropenen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Save', 'speichern', 'Сохранить', 'guardar', 'salve', 'salva', 'Enregistrer', 'зберегти', 'opslaan'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Save All', 'Alles speichern', 'Сохранить все', 'guardar todos', 'salve todas', 'salva tutte', 'Enregistrer toutes', 'зберегти всі', 'sla alles op'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Open All', 'Alles öffnen', 'Открыть все', 'abrir todos', 'abrir todas', 'aprire tutte', 'ouvrir tous', 'відчинено всі', 'Open alles'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Close', 'schließen', 'Закрыть', 'cerrar', 'feche', 'chiudi', 'fermer', 'закрити', 'sluiten'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Close All', 'Alles schließen', 'Закрыть все', 'cerrar todos', 'feche todas', 'chiudi tutte', 'fermer tous', 'закрити всі', 'sluit alles'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Exit', 'beenden', 'Выйти', 'salir', 'sair', 'uscire', 'quitter', 'вийти', 'verlaten'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'SQL file', 'SQL-Datei', 'SQL Файл', 'Archivo SQL', 'Arquivo SQL', 'File SQL', 'fichier SQL', 'SQL Файл', 'SQL bestand'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Function', 'Funktion', 'Функция', 'función', 'função', 'funzione', 'fonction', 'Функція', 'functie'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Procedure', 'Prozedur', 'Процедура', 'procedimiento', 'procedimento', 'procedura', 'procédure', 'процедури', 'Procedure'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Test', 'Prüfung', 'Тест', 'prueba', 'teste', 'Test', 'Test', 'випробування', 'Test'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Database', 'Datenbank', 'База данных', 'Base de datos', 'Base de dados', 'Banca dati', 'Base de données', 'База даних', 'Database'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Table', 'Tabelle', 'Таблица', 'Tabla', 'Tabela', 'Tabella', 'Tableau', 'Таблиця', 'Tabel'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Sequence', 'Sequenz', 'Последовательность', 'Secuencia', 'Seqüência', 'Sequenza', 'Séquence', 'Послідовність', 'Sequentie'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'View', 'Ansicht', 'Представление', 'Vista', 'Vista', 'Visualizzazione', 'Vue', 'Переглянути', 'Uitzicht'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Trigger', 'Auslöser', 'Триггер', 'Desencadenar', 'Acionar', 'Grilletto', 'Gâchette', 'Тригер', 'trigger'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Edit', 'Bearbeiten', 'Редактировать', 'Editar', 'Editar', 'Modificare', 'Éditer', 'Редагувати', 'Bewerking'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Undo', 'Rückgängig machen', 'Назад', 'Deshacer', 'Desfazer', 'Disfare', 'annuler', 'Скасувати', 'ongedaan maken'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Redo', 'Wiederholen', 'Вперед', 'Rehacer', 'Refazer', 'Rifare', 'Refaire', 'Повторити', 'Opnieuw doen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Settings', 'Einstellungen', 'Настройки', 'Configuración', 'Configurações', 'Impostazioni', 'Paramètres', 'Параметри', 'Instellingen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Pages', 'Seiten', 'Страницы', 'Paginas', 'Páginas', 'Pagine', 'pages', 'Сторінки', 'Pagina''s'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'SQL query', 'SQL-Abfrage', 'SQL запросы', 'consulta SQL', 'Consulta SQL', 'Query SQL', 'Requête SQL', 'SQL-запит', 'SQL-query'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Command line', 'Befehlszeile', 'Коммандная строка', 'Línea de comando', 'Linha de comando', 'Linea di comando', 'Ligne de commande', 'Командний рядок', 'Opdrachtregel'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Functions', 'Funktionen', 'Функции', 'Funciones', 'Funções', 'Funzioni', 'Les fonctions', 'Функції', 'Functies'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Procedures', 'Verfahren', 'Процедуры', 'Procedimientos', 'Procedimentos', 'Procedure', 'Procédures', 'Процедури', 'Procedures'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Tests', 'Tests', 'Тесты', 'Pruebas', 'Testes', 'Test', 'Essais', 'Тести', 'testen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Databases', 'Datenbanken', 'Базы данных', 'Bases de Datos', 'Bancos de dados', 'Banche dati', 'Bases de données', 'Бази даних', 'Databases'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Tables', 'Tabellen', 'Таблицы', 'Tablas', 'Tabelas', 'Tabelle', 'les tables', 'Таблиці', 'tabellen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Tools', 'Werkzeuge', 'Инструменты', 'Herramientas', 'Ferramentas', 'Strumenti', 'Outils', 'Інструменти', 'Gereedschap'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Options', 'Optionen', 'Опции', 'Opciones', 'Opções', 'Opzioni', 'Choix', 'Параметри', 'Opties'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Toolbars', 'Werkzeugkästen', 'Панель инструментов', 'Barras de herramientas', 'Barras de ferramentas', 'Barre degli strumenti', 'Barres d''outils', 'Панелі інструментів', 'Werkbalken'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Connections', 'Anschlüsse', 'Соединения', 'Conexiones', 'Conexões', 'Connessioni', 'Connexions', 'Підключення', 'verbindingen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Help', 'Hilfe', 'Помощь', 'Ayuda', 'Ajuda', 'Aiuto', 'Aider', 'Допомога', 'Helpen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Common SQL docs', 'Allgemeine SQL-Dokumentation', 'Общие документы по SQL', 'Documentos comunes de SQL', 'Documentos SQL comuns', 'Documenti SQL comuni', 'Documentation SQL commune', 'Загальні документи SQL', 'Algemene SQL-documenten'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'User guide', 'Benutzerhandbuch', 'Руководство пользователю', 'Guía del usuario', 'Guia de usuario', 'Guida utente', 'Manuel de l''utilisateur', 'Посібник користувача', 'Gebruikershandleiding'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'GitHub repository', 'GitHub-Repository', 'GitHub репозиторий', 'Repositorio de GitHub', 'Repositório do GitHub', 'Repository GitHub', 'Référentiel GitHub', 'Репозиторій GitHub', 'GitHub-opslagplaats'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Menu', 'Report', 'Prüfbericht', 'Отчет об ошибке', 'Informe', 'Relatório', 'Rapporto', 'Reportage', 'Звіт', 'Rapport'); 

INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Editor', 'Editor', 'Редактор', 'Editor', 'editor', 'Editore', 'Éditeur', 'Редактор', 'Editor'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Language', 'Sprache', 'Язык', 'Lenguaje', 'Linguagem', 'Lingua', 'Langue', 'Мову', 'Taal'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Auto save', 'Automatisch speichern', 'Автосохранение', 'Guardado automático', 'Salvamento automático', 'Salvataggio automatico', 'Sauvegarde automatique', 'Автозбереження', 'Automatisch opslaan'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Font size', 'Schriftgröße', 'Размер шрифта', 'Tamaño de fuente', 'Tamanho da fonte', 'Dimensione del font', 'Taille de police', 'Розмір шрифту', 'Lettertypegrootte'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Font family', 'Schriftfamilie', 'Шрифт', 'Familia tipográfica', 'Família de fontes', 'Famiglia di font', 'Famille de polices', 'Сімейство шрифтів', 'Lettertypefamilie'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Tab size', 'Tab-Größe', 'Размер табуляции', 'Tamaño de pestaña', 'Tamanho da guia', 'Dimensione della scheda', 'Taille de l''onglet', 'Розмір вкладки', 'Tabbladgrootte'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Word wrap', 'Zeilenumbruch', 'Перенос слов', 'Ajuste de línea', 'Quebra de linha', 'Involucro di parole', 'Retour à la ligne', 'Перенос слів', 'Woordterugloop'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'DB', 'DB', 'БД', 'base de datos', 'BD', 'DB', 'BD', 'БД', 'DB'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Default RDBMS', 'Standard-RDBMS', 'БД по умолчанию', 'RDBMS predeterminado', 'RDBMS padrão', 'RDBMS predefinito', 'SGBDR par défaut', 'СУБД за замовчуванням', 'Standaard RDBMS'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Active RDBMS', 'Aktives RDBMS', 'Активная БД', 'RDBMS activo', 'RDBMS ativo', 'RDBMS attivo', 'SGBDR actif', 'Активна СУБД', 'Actieve RDBMS'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Database', 'Datenbank', 'База данных', 'Base de datos', 'Base de dados', 'Banca dati', 'Base de données', 'База даних', 'Database'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Schema', 'Schema', 'Схема', 'Esquema', 'Esquema', 'Schema', 'Schéma', 'Схема', 'Schema'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Username', 'Nutzername', 'Пользователь', 'Nombre de usuario', 'Nome do usuário', 'Nome utente', 'Nom d''utilisateur', 'Ім''я користувача', 'gebruikersnaam'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Password', 'Passwort', 'Пароль', 'Contraseña', 'Senha', 'Parola d''ordine', 'Mot de passe', 'Пароль', 'Wachtwoord'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Recover', 'Wiederherstellung', 'Восстановить', 'Recuperar', 'Recuperar', 'Recuperare', 'Récupérer', 'Відновити', 'Herstellen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Save', 'Speichern', 'Сохранить', 'Salvar', 'Salvar', 'Salva', 'sauver', 'Зберегти', 'Opslaan'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Settings', 'Cancel', 'Stornieren', 'Отмена', 'Cancelar', 'Cancelar', 'Annulla', 'Annuler', 'Скасувати', 'Annuleren'); 

INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Common', 'Enabled', 'Ermöglicht', 'Включено', 'Activado', 'Habilitado', 'Abilitato', 'Autorisé', 'Увімкнено', 'Ingeschakeld'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Common', 'Disabled', 'Deaktiviert', 'Отключено', 'Discapacitado', 'Desabilitado', 'Disabilitato', 'Désactivé', 'Вимкнено', 'Gehandicapt'); 

INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Language', 'English', 'Englisch', 'Анлийский', 'inglés', 'inglês', 'inglese', 'Anglais', 'англійська', 'Engels'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Language', 'German', 'Deutsch', 'Немецкий', 'alemán', 'alemão', 'Tedesco', 'Allemand', 'німецька', 'Duits'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Language', 'Russian', 'Russisch', 'Русский', 'ruso', 'russo', 'russo', 'russe', 'російський', 'Russisch'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Language', 'Spanish', 'Spanisch', 'Испанский', 'español', 'espanhol', 'spagnolo', 'Espagnol', 'іспанська', 'Spaans'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Language', 'Portuguese', 'Portugiesisch', 'Португальский', 'portugués', 'português', 'portoghese', 'Portugais', 'португальська', 'Portugees'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Language', 'Italian', 'Italienisch', 'Итальянский', 'italiano', 'italiano', 'italiano', 'italien', 'італійська', 'Italiaans'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Language', 'French', 'Französisch', 'Французский', 'francés', 'francês', 'francese', 'français', 'французький', 'Frans'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Language', 'Ukrainian', 'ukrainisch', 'Украинский', 'ucranio', 'ucraniano', 'ucraino', 'ukrainien', 'український', 'Oekraïens'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Language', 'Dutch', 'Niederländisch', 'Нидерландский', 'holandés', 'holandês', 'olandese', 'Néerlandais', 'голландський', 'Nederlands'); 

INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Pages', 'Path', 'Pfad', 'Путь', 'Sendero', 'Caminho', 'Sentiero', 'Chemin', 'Шлях', 'Pad'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Pages', 'Execute', 'Ausführen', 'Выполнить', 'Ejecutar', 'Executar', 'Eseguire', 'Exécuter', 'Виконати', 'Uitvoeren'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Pages', 'Tables', 'Tabellen', 'Таблицы', 'Tablas', 'Tabelas', 'Tabelle', 'les tables', 'Таблиці', 'tabellen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Pages', 'General info', 'Allgemeine Information', 'Общие данные', 'Información general', 'Informações gerais', 'Informazioni generali', 'Informations générales', 'Загальна інформація', 'Algemene informatie'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Pages', 'Columns', 'Säulen', 'Столбцы', 'columnas', 'Colunas', 'Colonne', 'Colonnes', 'Колонки', 'Kolommen'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Pages', 'Foreign keys', 'Fremde Schlüssel', 'Внешние ключи', 'Llaves extranjeras', 'Chaves estrangeiras', 'Chiavi esterne', 'Clés étrangères', 'Сторонні ключі', 'Buitenlandse sleutels'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Pages', 'Triggers', 'Trigger', 'Триггеры', 'disparadores', 'Desencadeia', 'Trigger', 'Déclencheurs', 'Тригери', 'Triggers'); 
INSERT INTO translation (context, english, german, russian, spanish, portugues, italian, french, ukranian, dutch)
VALUES ('Pages', 'Data', 'Daten', 'Данные', 'Datos', 'Dados', 'Dati', 'Données', 'Дані', 'Gegevens'); 

