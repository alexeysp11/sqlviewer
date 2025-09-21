SELECT t.schemaname || '.' || t.relname AS name
FROM (SELECT schemaname, relname FROM pg_stat_user_tables) t
