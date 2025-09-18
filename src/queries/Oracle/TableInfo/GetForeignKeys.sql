SELECT * FROM all_constraints WHERE r_constraint_name IN
(
    SELECT constraint_name
    FROM all_constraints
    WHERE UPPER(table_name) LIKE UPPER('{0}')
)