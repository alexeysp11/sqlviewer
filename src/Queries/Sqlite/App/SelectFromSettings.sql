SELECT t.*
FROM (
    SELECT 
        CASE 
            WHEN UPPER(tmp.language) LIKE '%ENGISH%' THEN 'English' 
            WHEN UPPER(tmp.language) LIKE '%GERMAN%' THEN 'German' 
            WHEN UPPER(tmp.language) LIKE '%RUSSIAN%' THEN 'Russian' 
            WHEN UPPER(tmp.language) LIKE '%SPANISH%' THEN 'Spanish' 
            WHEN UPPER(tmp.language) LIKE '%PORTUGUESE%' THEN 'Portuguese' 
            WHEN UPPER(tmp.language) LIKE '%ITALIAN%' THEN 'Italian' 
            WHEN UPPER(tmp.language) LIKE '%FRENCH%' THEN 'French' 
            WHEN UPPER(tmp.language) LIKE '%UKRANIAN%' THEN 'Ukranian' 
            WHEN UPPER(tmp.language) LIKE '%DUTCH%' THEN 'Dutch' 
            WHEN UPPER(tmp.language) LIKE '%POLISH%' THEN 'Polish' 
            WHEN UPPER(tmp.language) LIKE '%CZECH%' THEN 'Czech' 
            WHEN UPPER(tmp.language) LIKE '%SERBIAN%' THEN 'Serbian' 
            WHEN UPPER(tmp.language) LIKE '%CROATIAN%' THEN 'Croatian' 
            WHEN UPPER(tmp.language) LIKE '%SWEDISH%' THEN 'Swedish' 
            WHEN UPPER(tmp.language) LIKE '%NORWEGIAN%' THEN 'Norwegian' 
            WHEN UPPER(tmp.language) LIKE '%DANISH%' THEN 'Danish' 
            WHEN UPPER(tmp.language) LIKE '%AFRIKAANS%' THEN 'Afrikaans' 
            WHEN UPPER(tmp.language) LIKE '%TURKISH%' THEN 'Turkish' 
            WHEN UPPER(tmp.language) LIKE '%KAZAKH%' THEN 'Kazakh' 
            WHEN UPPER(tmp.language) LIKE '%ARMENIAN%' THEN 'Armenian' 
            WHEN UPPER(tmp.language) LIKE '%GEORGIAN%' THEN 'Georgian' 
            WHEN UPPER(tmp.language) LIKE '%ROMANIAN%' THEN 'Romanian' 
            WHEN UPPER(tmp.language) LIKE '%BULGARIAN%' THEN 'Bulgarian' 
            WHEN UPPER(tmp.language) LIKE '%ALBANIAN%' THEN 'Albanian' 
            WHEN UPPER(tmp.language) LIKE '%GREEK%' THEN 'Greek' 
            WHEN UPPER(tmp.language) LIKE '%INDONESIAN%' THEN 'Indonesian' 
            WHEN UPPER(tmp.language) LIKE '%MALAY%' THEN 'Malay' 
            WHEN UPPER(tmp.language) LIKE '%KOREAN%' THEN 'Korean' 
            WHEN UPPER(tmp.language) LIKE '%JAPANESE%' THEN 'Japanese' 
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
            WHEN UPPER(tmp.default_rdbms) IN ('SQLITE', 'POSTGRESQL', 'MYSQL', 'ORACLE') THEN tmp.default_rdbms 
            ELSE 'SQLite'
        END AS default_rdbms, 
        CASE 
            WHEN UPPER(tmp.active_rdbms) IN ('SQLITE', 'POSTGRESQL', 'MYSQL', 'ORACLE') THEN tmp.active_rdbms 
            ELSE 'SQLite'
        END AS active_rdbms, 
        CAST(tmp.server AS TEXT) AS server, 
        CAST(tmp.db_name AS TEXT) AS db_name, 
        CAST(tmp.port AS TEXT) AS port, 
        CAST(tmp.schema_name AS TEXT) AS schema_name, 
        CAST(tmp.db_username AS TEXT) AS db_username, 
        CAST(tmp.db_pswd AS TEXT) AS db_pswd
    FROM (
        SELECT 
            MAX(tt.language) AS language, 
            MAX(tt.auto_save) AS auto_save, 
            MAX(tt.font_size) AS font_size, 
            MAX(tt.font_family) AS font_family, 
            MAX(tt.tab_size) AS tab_size, 
            MAX(tt.word_wrap) AS word_wrap, 
            MAX(tt.default_rdbms) AS default_rdbms, 
            MAX(tt.active_rdbms) AS active_rdbms, 
            MAX(tt.server) AS server, 
            MAX(tt.db_name) AS db_name, 
            MAX(tt.port) AS port, 
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
                    WHEN UPPER(t.name) LIKE '%SERVER%' THEN COALESCE(t.value, '')
                    ELSE ''
                END AS server, 
                CASE  
                    WHEN UPPER(t.name) LIKE '%DATABASE%' THEN COALESCE(t.value, '')
                    ELSE ''
                END AS db_name, 
                CASE  
                    WHEN UPPER(t.name) LIKE '%PORT%' THEN COALESCE(t.value, '')
                    ELSE ''
                END AS port, 
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
    ) tmp 
    LIMIT 1
) t; 
