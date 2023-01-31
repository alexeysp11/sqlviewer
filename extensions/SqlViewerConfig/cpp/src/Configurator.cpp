#include <fstream>
#include <iostream>
#include <windows.h>
#include "../headers/Configurator.hpp"

void Configurator::CreateFolders()
{
    char path[] = "C:\\Users\\123\\Documents\\projects\\sqlviewer\\extensions\\SqlViewerConfig\\cpp\\bin\\tmp"; 
    if (CreateDirectory(path, NULL) || ERROR_ALREADY_EXISTS == GetLastError())
    {
        // CopyFile(...)
        //std::cout << "Created" << std::endl; 
    }
    else
    {
        // Failed to create directory.
        //std::cout << "Failed to create directory" << std::endl; 
    }
}

void Configurator::CreateLocalFiles()
{
    char path[] = "C:\\Users\\123\\Documents\\projects\\sqlviewer\\extensions\\SqlViewerConfig\\cpp\\bin\\tmp\\Hello.txt"; 
    if (!IsFileExists(path)) 
    {
        std::ofstream { path };
    }
}

void Configurator::InitDatabases()
{
    std::cout << "InitDatabases" << std::endl; 
}

bool Configurator::IsFileExists(const char filename[]) 
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

