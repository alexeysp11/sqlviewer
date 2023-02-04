#ifndef SQLVIEWERDATABASE_DLLINTERFACE_H
#define SQLVIEWERDATABASE_DLLINTERFACE_H

#ifdef __cplusplus 
extern "C" {
#endif 

#ifdef BUILD_MY_DLL
#define SQLVIEWER_API __declspec(dllexport)
#else
#define SQLVIEWER_API __declspec(dllimport)
#endif 

int SQLVIEWER_API Add(int a, int b);
void SQLVIEWER_API RunSqliteDllInterface(char* table, char* sql); 

#ifdef __cplusplus 
}
#endif 

#endif  // SQLVIEWERDATABASE_DLLINTERFACE_H
