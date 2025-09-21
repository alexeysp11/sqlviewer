CREATE OR REPLACE FUNCTION fGetSqlFromTable(aSchemaName VARCHAR(255), aTableName VARCHAR(255)) 
RETURNS TEXT 
LANGUAGE plpgsql AS
$func$
DECLARE 
    i INTEGER; 
    lNumRec INTEGER; 
    rec RECORD;
    lResult text;
BEGIN
    i := 0; 
    SELECT COUNT(*) INTO lNumRec FROM information_schema.columns WHERE table_schema LIKE aSchemaName AND table_name LIKE aTableName; 
    lResult := 'CREATE TABLE ' || aSchemaName || '.' || aTableName || chr(10) || '(' || chr(10); 
    FOR rec IN (
        SELECT 
            column_name, 
            column_default, 
            is_nullable, 
            data_type, 
            character_maximum_length
        FROM information_schema.columns 
        WHERE table_schema LIKE aSchemaName AND table_name LIKE aTableName
    )
    LOOP
        i := i + 1; 
        lResult := lResult || '    ' || rec.column_name || ' ' || rec.data_type; 
        IF UPPER(rec.data_type) LIKE '%CHAR%VAR%' THEN 
            lResult := lResult || '(' || rec.character_maximum_length || ')'; 
        END IF; 
        IF rec.column_default IS NOT NULL AND rec.column_default <> '' THEN 
            lResult := lResult || ' DEFAULT ' || rec.column_default; 
        END IF; 
        IF i = lNumRec THEN 
            lResult := lResult || chr(10) || ');'; 
        ELSE 
            lResult := lResult || ', ' || chr(10); 
        END IF; 
    END LOOP;

    RETURN lResult; 
END
$func$;

SELECT fGetSqlFromTable('{0}', '{1}') AS sql; 
