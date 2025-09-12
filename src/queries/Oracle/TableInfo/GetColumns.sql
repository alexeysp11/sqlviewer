SELECT column_name, data_type, data_length
FROM USER_TAB_COLUMNS
WHERE UPPER(table_name) = UPPER('{0}')