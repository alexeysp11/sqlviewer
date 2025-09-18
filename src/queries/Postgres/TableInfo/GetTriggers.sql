SELECT *
FROM information_schema.triggers
WHERE event_object_table LIKE '{0}'
