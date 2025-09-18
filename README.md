# sqlviewer 

[English](README.md) | [Русский](README.ru.md)

`sqlviewer` is a multilingual C# application that provides a user-friendly interface for interacting with different RDBMS and performing various database operations.

## Overall description 

This project is a C# implementation of a GUI for retrieving and transfering data from the following RDBMS: 
- **SQLite**,
- **PostgreSQL**,
- **MySQL**,
- **Oracle**. 

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

Using this app, you can do the following things: 

- write and execute SQL queries:

![Example (UI, query)](docs/img/ui_query.png)

- watch information about all tables inside your database (SQL definition, columns, foreign keys, triggers and all data inside a paticular table): 

![Example (UI, tables)](docs/img/ui_tables.png)

- transfer data from one database to another:

![Example (UI, connections)](docs/img/ui_connections.png)

### Goal

The goal of the project is to create a C# GUI application for retrieving and transferring data from various RDBMS, including SQLite, PostgreSQL, MySQL, and Oracle.

The scope of the project includes implementing a GUI for executing SQL queries, viewing database information, and transferring data between different databases.

This project can be used by database administrators, developers, and anyone who needs to work with multiple RDBMS and perform data management tasks.

### Similar open-source projects

Similar open-source projects include [DBeaver](https://github.com/dbeaver/dbeaver) and [SQuirreL SQL](https://github.com/squirrel-sql-client), but there are also similar projects written in C# such as [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) and [LINQPad](http://linqpad.net/).

### Possible limitations

Possible limitations of this project could include compatibility issues with certain RDBMS, performance issues when dealing with large datasets, and potential security vulnerabilities when transferring data between databases.

## Getting started

### Prerequisites

- Windows OS; 
- .NET Core 3.1; 
- One of the following data sources to be able to perform some operations with data: 
    - **SQLite**,
    - **PostgreSQL**,
    - **MySQL**,
    - **Oracle**.

### How to run 

In order to run the application, you can use command line: 
1. Go to the main folder of the repository: 
```
cd C:\PathToRepo\sqlviewer 
```
2. Execute `config.cmd` file to restore all the projects, and initialize databases and the project's file system: 
```
config.cmd
```
3. Execute `run.cmd` file: 
```
run.cmd 
```

### How to use 

[Click here](docs/HowToUse.md) to read guide on how to use the application. 

## For developers 

This application is written in C# with **WPF** using **MVVM** pattern. 

### How to contribute

1. [Click here](https://docs.github.com/en/get-started/quickstart/contributing-to-projects) to read guide on how to contribute to GitHub projects (common for any GitHub project). 
2. Read [to-do list](docs/ToDoList.md). 

### Application structure 

Class diagram is shown below:

![Class diagram: SqlViewer](docs/img/sqlviewer_classdiagram.png)
