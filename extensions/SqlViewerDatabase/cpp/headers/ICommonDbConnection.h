#ifndef SQLVIEWERDATABASE_ICOMMONDBCONNECTION_HPP
#define SQLVIEWERDATABASE_ICOMMONDBCONNECTION_HPP

static int callback(void *NotUsed, int argc, char **argv, char **azColName) ; 
int exec_sqlite(); 

#endif  // SQLVIEWERDATABASE_ICOMMONDBCONNECTION_HPP
