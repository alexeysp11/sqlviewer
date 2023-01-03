CREATE TABLE IF NOT EXISTS settings
(
    settings_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    name VARCHAR(25) NOT NULL, 
    description VARCHAR(100), 
    value VARCHAR(100)
); 
CREATE TABLE IF NOT EXISTS tmp_settings
(
    settings_id INTEGER, 
    name VARCHAR(25) NOT NULL, 
    description VARCHAR(100), 
    value VARCHAR(100)
); 

INSERT INTO settings (name) SELECT 'language' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'language') = 0; 
INSERT INTO settings (name) SELECT 'auto_save' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'auto_save') = 0; 
INSERT INTO settings (name) SELECT 'font_size' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'font_size') = 0; 
INSERT INTO settings (name) SELECT 'font_family' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'font_family') = 0; 
INSERT INTO settings (name) SELECT 'tab_size' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'tab_size') = 0; 
INSERT INTO settings (name) SELECT 'word_wrap' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'word_wrap') = 0; 
INSERT INTO settings (name) SELECT 'default_rdbms' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'default_rdbms') = 0; 
INSERT INTO settings (name) SELECT 'active_rdbms' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'active_rdbms') = 0; 
INSERT INTO settings (name) SELECT 'server' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'server') = 0;
INSERT INTO settings (name) SELECT 'database' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'database') = 0;
INSERT INTO settings (name) SELECT 'port' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'port') = 0;
INSERT INTO settings (name) SELECT 'schema' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'schema') = 0; 
INSERT INTO settings (name) SELECT 'username' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'username') = 0; 
INSERT INTO settings (name) SELECT 'password' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'password') = 0; 

DELETE FROM settings WHERE name IN (SELECT name FROM settings GROUP BY name HAVING COUNT(*) > 1); 
