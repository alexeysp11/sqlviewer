# To-do list 

1. DbConnections:
    - Add MS SQL. 
    - Add MongoDB. 
    - Add PostgreSQL, MySQL, MS SQL and Oracle (pure SQL and EntityFramework) for AppDbConnection. 
    - Use parameters for SQL queries. 
2. Data transferring and data processing: 
    - When you transfer data to another database, ask user if they want the table structure to be the same as in source database.  
3. Logging: 
    - Add NLog, log4net and Serilog; 
    - Add PostgreSQL, MySQL and Oracle for logging. 
4. Extensions: 
    - In config extension, check if all folders and files were created. 
    - In config extension list all of the files, folders and databases in `appconfig.json`. 
5. Translation: 
    - Translate all the elements that were not translated (e.g. server, port etc). 
    - Add new languages. 
    - Context from translation table may be stored in separate table; try to use transaction for db initialization. 
    - Translate messages and commonly used words (Enabled, Disabled etc) to each of the languages (add parameter in config file to get if the messages should be displayed in english or in the language that was selected by user). 
6. Structure diagrams:
    - Add AppMainCommand to the class diagram. 
    - Add web version of the application (in this case you should write an additional module SqlViewerStartup to read the config file and run web or desktop app respectively, plus to separate WPF specific functionality from core functionality of the application). 
7. UI: 
    - Implement UI representation for right to left scripts. 
8. Network: 
    - Add server for reporting bugs and errors. 
    - Use email. 
9. Languages: 
    - Arabic; 
    - Catalan; 
    - Chinese (simplified); 
    - Chinese (traditional); 
    - Estonian; 
    - Filipino; 
    - Finnish; 
    - Hebrew; 
    - Hindi; 
    - Hungarian; 
    - Icelandic; 
    - Irish; 
    - Kyrgyz; 
    - Latvian; 
    - Lithuanian; 
    - Maltese; 
    - Mongolian; 
    - Nepali; 
    - Persian; 
    - Thai; 
    - Uzbek;  
    - Vietnamese;  
    - Welsh; 
    - Yidish. 
10. Common: 
    - Add tests. 
    - Check if you create some of the objects more than once.
    - Use layer diagram to represent what your application consists of. 
    - Try to reduce repeating code. 
    - Use appconfig.json to notify a user about necessety of configuration of data folder for executing the program. 
    - Give some explanations on how to add a new database, language or UI element into the application, how to translate UI element. 
    - Remove unnecessary usings. 
    - Display exceptions message (and stack trace) if the corresponding parameter is set in config file. 
