# sqlviewer 

`sqlviewer` is a C# implementation of a GUI for retrieving and transfering data from the following sources: 
- RDBMS (**SQLite**, **PostgreSQL**, **MySQL** and **Oracle**);  
- Custom **JSON**/**XML**/**CSV** files; 
- MS Office (**Excel**); 
- LibreOffice/OpenOffice (**Calc**). 

Web service features: 
- HTTP, FTP, TCP; 
- SOAP/WCF, gRPC, RESTful API; 
- NNTP, IPFS; 
- Instant messaging protocols (IMAP, IRC, POP, SMTP, XMPP, MQTP). 

It's available in 29 different languages, such as: 
- English;
- German;
- Russian;
- Spanish;
- Portuguese;
- Italian;
- French;
- Ukranian;
- Dutch;
- Polish;
- Czech;
- Serbian;
- Croatian;
- Korean;
- Japanese, etc. 

<!--
Add some more languages: 
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
-->

Using this app, you can do the following things: 

- write and execute SQL queries:

![Example (UI, query)](docs/img/ui_query.png)

- watch information about all tables inside your database (SQL definition, columns, foreign keys, triggers and all data inside a paticular table): 

![Example (UI, tables)](docs/img/ui_tables.png)

- transfer data from one database to another:

![Example (UI, connections)](docs/img/ui_connections.png)

- connect to other computers and send data over the network:

![Example (UI, network)](docs/img/ui_network.png)

- get data from MS Excel or LibreOffice/OpenOffice Calc, and save it to a specific database: 

![Example (UI, custom files)](docs/img/custom_files.png)

## Getting started 

### Prerequisites 

- Windows OS; 
- .NET Core 3.1; 
- One of the following data sources to be able to perform some operations with data (if you have more than one data sources installed on your computer, your user experience is going to be much better and productive): 
    - RDBMS (**SQLite**, **PostgreSQL**, **MySQL**, or **Oracle**), 
    - MS Office (**Excel**), 
    - LibreOffice/OpenOffice (**Calc**). 

### Download and run 

To be continued... 

### How to use 

<!--
- Running the application; 
- Configuration; 
- User manual. 
-->

To be continued... 

## For developers 

This application is written in C# with **WPF** using **MVVM** pattern. 

### Application structure 

Class diagram is shown below:

![Example (UI, query)](docs/img/sqlviewer_diagram.png)
