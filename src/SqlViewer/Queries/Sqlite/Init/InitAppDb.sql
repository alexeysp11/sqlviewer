-- Create tables 
CREATE TABLE sys_param 
(
    -- stores information about that how the app works (filter strings for opening/creating a file, )
	sys_param_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
	name VARCHAR(50) NOT NULL, 
    short_name VARCHAR(25), 
    value VARCHAR(255) NOT NULL
); 

CREATE TABLE sys_sql
(
    sys_sql_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    name VARCHAR(50) NOT NULL, 
    sql TEXT NOT NULL
); 

CREATE TABLE command 
(
    command_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    name VARCHAR(50) NOT NULL,              -- name of a command
    in_param VARCHAR(50) NOT NULL,          -- name of a parameter that passed into a Command from View
    data_type VARCHAR(50),                  -- Data type of a created instance (if you need to create a View)
    msg VARCHAR(50),                        -- message that should appear in an opened window 
    title VARCHAR(50),                      -- title of an opened window (if command's supposed to open a window)
    path VARCHAR(255),                      -- path of a doc file (if command's supposed to open a doc)
    out_class VARCHAR(50) NOT NULL,         -- name of a ViewModel class 
    out_method VARCHAR(50) NOT NULL,        -- method that we call in ViewModel
    out_type NUMERIC(1)                     -- 0 - void, 1 - path, 2 - , 3 - only path 
);

/* 
SELECT 
    out_class, 
    out_method, 
    WHEN 
        CASE out_param_type = 0 THEN NULL  
        CASE out_param_type = 1 THEN msg 
        CASE out_param_type = 2 THEN title 
        CASE out_param_type = 3 THEN path 
    END AS out_param  
FROM command 
WHERE name = '{0}' AND in_param = '{1}'; 
*/ 


CREATE TABLE user 
(
    user_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    name VARCHAR(25) NOT NULL, 
    pswd VARCHAR(50), 
    role_id INTEGER 
); 

CREATE TABLE role 
(
    role_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    name VARCHAR(25), 
    f_read NUMERIC(1), 
    f_write NUMERIC(1), 
    f_edit NUMERIC(1), 
    f_delete NUMERIC(1) 
); 

CREATE TABLE user_settings 
(
    user_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    settings_id INTEGER NOT NULL, 
    value INTEGER NOT NULL
); 

/*
Necessary tables: 
- Settings (key, value). 
- Parameters (parameter_id, name, short_name, value): 
    - Filters for opening/creating files. 
*/
