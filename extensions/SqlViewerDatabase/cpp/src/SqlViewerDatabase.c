#include <stdio.h>
#include "../headers/dllinterface.h"
#include "../headers/ICommonDbConnection.h"
#include "../../../../packages/sqlite3/src/sqlite3.h"

int SQLVIEWER_API Add(int a, int b)
{
    return (a + b);
}

void SQLVIEWER_API RunSqliteDllInterface(char* table, char* sql)
{
    exec_sqlite(table, sql); 
}

static int callback(void *NotUsed, int argc, char **argv, char **azColName) 
{
    for(int i = 0; i < argc; i++) 
    {
        printf("%s = %s\n", azColName[i], argv[i] ? argv[i] : "NULL");
    }
    printf("\n");
    return 0;
}

int exec_sqlite(char* table, char* sql)
{
    sqlite3 *db;
    char *zErrMsg = 0;
    int rc;
  
    rc = sqlite3_open(table, &db);
    if(rc)
    {
        fprintf(stderr, "Can't open database '%s': %s\n", table, sqlite3_errmsg(db));
        sqlite3_close(db);
        return(1);
    }
    rc = sqlite3_exec(db, sql, callback, 0, &zErrMsg);
    if(rc != SQLITE_OK)
    {
        fprintf(stderr, "SQL error: %s\n", zErrMsg);
        sqlite3_free(zErrMsg);
    }
    sqlite3_close(db);

    return 0; 
}
