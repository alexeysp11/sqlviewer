select dbms_metadata.get_ddl('TABLE', table_name) as sql
from user_tables ut
WHERE UPPER(ut.table_name) LIKE UPPER('{0}')
