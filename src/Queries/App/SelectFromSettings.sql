WITH tmp AS (
    SELECT 
        MAX(tt.language) AS language, 
        MAX(tt.auto_save) AS auto_save, 
        MAX(tt.font_size) AS font_size, 
        MAX(tt.font_family) AS font_family, 
        MAX(tt.tab_size) AS tab_size, 
        MAX(tt.word_wrap) AS word_wrap, 
        MAX(tt.default_rdbms) AS default_rdbms, 
        MAX(tt.active_rdbms) AS active_rdbms, 
        MAX(tt.db_name) AS db_name, 
        MAX(tt.schema_name) AS schema_name, 
        MAX(tt.db_username) AS db_username,
        MAX(tt.db_pswd) AS db_pswd
    FROM (
        SELECT 
            CASE  
                WHEN UPPER(t.name) LIKE '%LANG%' OR UPPER(t.name) LIKE '%LANGUAGE%' THEN COALESCE(t.value, 'English')
                ELSE ''
            END AS language, 
            CASE  
                WHEN UPPER(t.name) LIKE '%AUTOSAVE%' OR UPPER(t.name) LIKE '%AUTO_SAVE%' THEN COALESCE(t.value, 'Enabled')
                ELSE ''
            END AS auto_save, 
            CASE  
                WHEN UPPER(t.name) LIKE '%FONT_SIZE%' THEN COALESCE(t.value, '8')
                ELSE ''
            END AS font_size, 
            CASE  
                WHEN UPPER(t.name) LIKE '%FONT_FAMILY%' THEN COALESCE(t.value, 'Consolas')
                ELSE ''
            END AS font_family, 
            CASE  
                WHEN UPPER(t.name) LIKE '%TAB_SIZE%' THEN COALESCE(t.value, '8')
                ELSE ''
            END AS tab_size, 
            CASE  
                WHEN UPPER(t.name) LIKE '%WORD_WRAP%' THEN COALESCE(t.value, 'Enabled')
                ELSE ''
            END AS word_wrap, 
            CASE 
                WHEN UPPER(t.name) LIKE '%DEFAULT_RDBMS%' THEN COALESCE(t.value, 'SQLite')
                ELSE ''
            END AS default_rdbms, 
            CASE  
                WHEN UPPER(t.name) LIKE '%ACTIVE_RDBMS%' THEN COALESCE(t.value, 'SQLite')
                ELSE ''
            END AS active_rdbms, 
            CASE  
                WHEN UPPER(t.name) LIKE '%DATABASE%' THEN COALESCE(t.value, '')
                ELSE ''
            END AS db_name, 
            CASE  
                WHEN UPPER(t.name) LIKE '%SCHEMA%' THEN COALESCE(t.value, '')
                ELSE ''
            END AS schema_name, 
            CASE  
                WHEN UPPER(t.name) LIKE '%USERNAME%' THEN COALESCE(t.value, '')
                ELSE ''
            END AS db_username, 
            CASE  
                WHEN UPPER(t.name) LIKE '%PASSWORD%' OR UPPER(t.name) LIKE '%PSWD%' THEN COALESCE(t.value, '')
                ELSE ''
            END AS db_pswd
        FROM (
            SELECT DISTINCT s.name, s.value 
            FROM settings s 
            WHERE s.name IS NOT NULL 
        ) t 
    ) tt 
) 
SELECT 
    CASE 
        WHEN UPPER(tmp.language) LIKE '%ENGISH%' THEN 'English' 
        WHEN UPPER(tmp.language) LIKE '%GERMAN%' THEN 'German' 
        WHEN UPPER(tmp.language) LIKE '%RUSSIAN%' THEN 'Russian' 
        WHEN UPPER(tmp.language) LIKE '%SPANISH%' THEN 'Spanish' 
        WHEN UPPER(tmp.language) LIKE '%PORTUGUES%' THEN 'Portugues' 
        WHEN UPPER(tmp.language) LIKE '%ITALIAN%' THEN 'Italian' 
        WHEN UPPER(tmp.language) LIKE '%FRENCH%' THEN 'French' 
        WHEN UPPER(tmp.language) LIKE '%UKRANIAN%' THEN 'Ukranian' 
        WHEN UPPER(tmp.language) LIKE '%DUTCH%' THEN 'Dutch' 
        ELSE 'English'
    END AS language, 
    CASE 
        WHEN UPPER(tmp.auto_save) LIKE '%ENABLED%' OR UPPER(tmp.auto_save) LIKE '1' THEN 'Enabled' 
        ELSE 'Disabled'
    END AS auto_save, 
    CASE 
        WHEN CAST(tmp.font_size AS INTEGER) IN (8,9,10,11,12,14,16,18,20) THEN CAST(tmp.font_size AS INTEGER) 
        ELSE 8
    END AS font_size, 
    CASE 
        WHEN UPPER(tmp.font_family) IN ('CONSOLAS') THEN tmp.font_family 
        ELSE 'Consolas'
    END AS font_family, 
    CASE 
        WHEN CAST(tmp.tab_size AS INTEGER) BETWEEN 1 AND 8 THEN CAST(tmp.tab_size AS INTEGER) 
        ELSE 4
    END AS tab_size, 
    CASE 
        WHEN UPPER(tmp.word_wrap) LIKE '%ENABLED%' OR UPPER(tmp.word_wrap) LIKE '1' THEN 'Enabled' 
        ELSE 'Disabled'
    END AS word_wrap, 
    CASE 
        WHEN UPPER(tmp.default_rdbms) IN ('SQLITE', 'POSTGRESQL', 'MYSQL') THEN tmp.default_rdbms 
        ELSE 'SQLite'
    END AS default_rdbms, 
    CASE 
        WHEN UPPER(tmp.active_rdbms) IN ('SQLITE', 'POSTGRESQL', 'MYSQL') THEN tmp.active_rdbms 
        ELSE 'SQLite'
    END AS active_rdbms, 
    CAST(tmp.db_name AS TEXT) AS db_name, 
    CAST(tmp.schema_name AS TEXT) AS schema_name, 
    CAST(tmp.db_username AS TEXT) AS db_username, 
    CAST(tmp.db_pswd AS TEXT) AS db_pswd
FROM tmp 
LIMIT 1; 
