UPDATE settings SET value = 'English' WHERE name LIKE 'language'; 
UPDATE settings SET value = 'Enabled' WHERE name LIKE 'auto_save'; 
UPDATE settings SET value = '8' WHERE name LIKE 'font_size'; 
UPDATE settings SET value = 'Consolas' WHERE name LIKE 'font_family'; 
UPDATE settings SET value = '4' WHERE name LIKE 'tab_size'; 
UPDATE settings SET value = '8' WHERE name LIKE 'word_wrap'; 
UPDATE settings SET value = 'SQLite' WHERE name LIKE 'default_rdbms'; 
UPDATE settings SET value = 'SQLite' WHERE name LIKE 'active_rdbms'; 
UPDATE settings SET value = '' WHERE name LIKE 'database'; 
UPDATE settings SET value = '' WHERE name LIKE 'schema'; 
UPDATE settings SET value = '' WHERE name LIKE 'username'; 
UPDATE settings SET value = '' WHERE name LIKE 'password'; 