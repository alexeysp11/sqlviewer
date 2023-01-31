SELECT 
    column_name, 
    ordinal_position, 
    column_default, 
    is_nullable, 
    data_type, 
    is_self_referencing, 
    is_generated, 
    is_updatable
FROM information_schema.columns 
WHERE table_schema LIKE '{0}' AND table_name LIKE '{1}'
