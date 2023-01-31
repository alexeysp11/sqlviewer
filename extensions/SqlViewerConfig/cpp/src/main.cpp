#include <iostream>
#include "../headers/Configurator.hpp"

int main()
{
    Configurator configurator; 

    std::cout << "Create folders..." << std::endl; 
    configurator.CreateFolders(); 
    std::cout << "Folders are created." << std::endl; 

    std::cout << "Create local files..." << std::endl; 
    configurator.CreateLocalFiles(); 
    std::cout << "Local files are created." << std::endl; 

    //std::cout << "Initialize databases..." << std::endl; 
    //configurator.InitDatabases(); 
    //std::cout << "Databases are initialized." << std::endl; 

    std::cout << std::endl << "Press any key to continue..." << std::endl; 

    return 0; 
}
