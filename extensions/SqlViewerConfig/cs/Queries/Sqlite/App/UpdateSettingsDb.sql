UPDATE settings SET value = '{0}' WHERE UPPER(name) LIKE 'DEFAULT_RDBMS'; 
UPDATE settings SET value = '{1}' WHERE UPPER(name) LIKE 'ACTIVE_RDBMS'; 
UPDATE settings SET value = '{2}' WHERE UPPER(name) LIKE 'SERVER'; 
UPDATE settings SET value = '{3}' WHERE UPPER(name) LIKE 'DATABASE'; 
UPDATE settings SET value = '{4}' WHERE UPPER(name) LIKE 'PORT'; 
UPDATE settings SET value = '{5}' WHERE UPPER(name) LIKE 'SCHEMA'; 
UPDATE settings SET value = '{6}' WHERE UPPER(name) LIKE 'USERNAME'; 
UPDATE settings SET value = '{7}' WHERE UPPER(name) LIKE 'PASSWORD'; 
