#ifndef SQLVIEWERCONFIG_CONFIGURATOR_HPP
#define SQLVIEWERCONFIG_CONFIGURATOR_HPP

#include <fstream>
#include <iostream>
#include <stdio.h>
#include <string.h>
#include <windows.h>
#include <exception>
#include <typeinfo>
#include <stdexcept>
#include "../headers/Configurator.hpp"

namespace SqlViewerConfig
{
    class Configurator
    {
    public: 
        void CreateFolders(); 
        void CreateLocalFiles(); 
        void InitDatabases(); 
    private: 
        bool IsFileExists(const char filename[]); 
        char* GetRootFolder(); 
        char* ConcatenateCharArrays(char* str1, char* str2); 
    }; 
}

#endif  // SQLVIEWERCONFIG_CONFIGURATOR_HPP
