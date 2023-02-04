#include <iostream>
#include "../headers/Configurator.hpp"
#include "../../../SqlViewerDatabase/cpp/headers/dllinterface.h"

int main()
{
    SqlViewerConfig::Configurator configurator; 

    std::cout << "Create folders..." << std::endl; 
    configurator.CreateFolders(); 
    std::cout << "Folders are created." << std::endl; 

    std::cout << "Create local files..." << std::endl; 
    configurator.CreateLocalFiles(); 
    std::cout << "Local files are created." << std::endl; 

    std::cout << "Initialize databases..." << std::endl; 
    configurator.InitDatabases(); 
    std::cout << "Databases are initialized." << std::endl; 

    std::cout << std::endl << "Configuration is finished successfully!" << std::endl; 

    return 0; 
}

void SqlViewerConfig::Configurator::CreateFolders()
{
    try 
    {
        char* root = GetRootFolder(); 
        char* paths[] = {
            ConcatenateCharArrays(root, "data"), 
            ConcatenateCharArrays(root, "data/SqlViewerNetwork"),
            ConcatenateCharArrays(root, "data/SqlViewer"),
            ConcatenateCharArrays(root, "data/!backups")
        }; 
        for (int i = 0; i < sizeof(paths)/sizeof(paths[0]); i++)
        {
            CreateDirectory(paths[i], NULL); 
        }
    }
    catch(...)
    {
        std::exception_ptr p = std::current_exception();
        std::cout << (p ? p.__cxa_exception_type()->name() : "null") << std::endl;
    }
}

void SqlViewerConfig::Configurator::CreateLocalFiles()
{
    try 
    {
        char* root = GetRootFolder(); 
        char* paths[] = {
            ConcatenateCharArrays(root, "data/app.db"), 
            ConcatenateCharArrays(root, "data/data.db"), 
            ConcatenateCharArrays(root, "data/test.db"), 
            ConcatenateCharArrays(root, "data/test1.db"), 
            ConcatenateCharArrays(root, "data/test2.db"), 
            ConcatenateCharArrays(root, "data/SqlViewerNetwork/settings.xml")
        }; 
        for (int i = 0; i < sizeof(paths)/sizeof(paths[0]); i++)
        {
            if (!IsFileExists(paths[i])) 
            {
                std::ofstream { paths[i] };
            }
        }
    }
    catch(...)
    {
        std::exception_ptr p = std::current_exception();
        std::cout << (p ? p.__cxa_exception_type()->name() : "null") << std::endl;
    }
}

void SqlViewerConfig::Configurator::InitDatabases()
{
    char* table = "C:\\path_to_project\\sqlviewer\\data\\test1.db"; 
    char* sql = "CREATE TABLE IF NOT EXISTS names (name_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name VARCHAR(50) NOT NULL); INSERT INTO names (name) VALUES ('Mark'); INSERT INTO names (name) VALUES ('Steve'); INSERT INTO names (name) VALUES ('Brian'); "; 
    RunSqliteDllInterface(table, sql); 
}

bool SqlViewerConfig::Configurator::IsFileExists(const char filename[]) 
{
    if (FILE *file = fopen(filename, "r")) 
    {
        fclose(file);
        return true;
    } 
    else 
    {
        return false;
    }   
}

char* SqlViewerConfig::Configurator::GetRootFolder()
{
    std::string rootFolder = "Failed to get path";

    char selfp[MAX_PATH];
    DWORD szPath;
    szPath = GetModuleFileName(NULL, selfp, MAX_PATH);
    if (szPath != 0) // successfully got path of current program
    {
        std::string helper = selfp;
        size_t pos = helper.find_last_of( "\\" );
        if (pos != std::string::npos) // found last backslash
        {
            rootFolder = helper.substr(0, pos + 1);
            rootFolder += "..\\..\\..\\..\\";
        }
    }
    char* tmp = (char*)(rootFolder.c_str()); 
    char* output = (char*)malloc(sizeof(tmp)/sizeof(tmp[0]));
    sprintf((char*)output, "%s", tmp);

    return output; 
}

char* SqlViewerConfig::Configurator::ConcatenateCharArrays(char* str1, char* str2) 
{
    size_t length1 = 0, length2 = 0;
    
    while(str1[length1] != '\0') length1++;
    while(str2[length2] != '\0') length2++;
    char* output = (char*)malloc(length1 + length2);
    sprintf((char*)output, "%s%s", str1, str2);

    return output;
}
