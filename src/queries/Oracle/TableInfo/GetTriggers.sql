SELECT *
FROM all_triggers
WHERE UPPER(table_name) LIKE UPPER('{0}')