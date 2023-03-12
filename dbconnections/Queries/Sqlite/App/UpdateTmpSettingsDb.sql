INSERT INTO tmp_settings (name) SELECT LOWER('{0}') WHERE (SELECT COUNT(*) FROM tmp_settings WHERE UPPER(name) LIKE UPPER('{0}')) = 0; 
DELETE FROM tmp_settings WHERE name IN (SELECT name FROM tmp_settings GROUP BY name HAVING COUNT(*) > 1); 
UPDATE tmp_settings SET value = '{1}' WHERE UPPER(name) LIKE UPPER('{0}'); 
