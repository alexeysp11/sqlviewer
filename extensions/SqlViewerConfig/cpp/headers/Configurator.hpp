#ifndef SQLVIEWERCONFIG_CONFIGURATOR_HPP
#define SQLVIEWERCONFIG_CONFIGURATOR_HPP

class Configurator
{
public: 
    void CreateFolders(); 
    void CreateLocalFiles(); 
    void InitDatabases(); 
private: 
    bool IsFileExists(const char filename[]); 
}; 

#endif  // SQLVIEWERCONFIG_CONFIGURATOR_HPP
