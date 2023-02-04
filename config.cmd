@echo off 

echo Configuration is started 

echo Restoring C# projects 
dotnet restore ./extensions/SqlViewerDatabase/cs/SqlViewerDatabase.csproj
dotnet restore ./extensions/SqlViewerConfig/cs/SqlViewerConfig.csproj
dotnet restore ./extensions/SqlViewerNetwork/cs/SqlViewerNetwork.csproj
dotnet restore ./src/SqlViewer.csproj
echo All C# projects are restored  

echo Downloading C/C++ external libraries 
if not exist packages mkdir "packages" 

echo sqlite3...
if not exist packages/sqlite3 mkdir "packages\sqlite3" 
if not exist packages/sqlite3/src mkdir "packages\sqlite3\src" 
if not exist packages/sqlite3/bin mkdir "packages\sqlite3\bin" 
if exist packages/sqlite3/src/shell.c (
    if exist packages/sqlite3/src/sqlite3.c (
        if exist packages/sqlite3/src/sqlite3.h (
            goto :sqlitedownloaded 
        ) else goto :downloadsqlite 
    ) else goto :downloadsqlite 
) else goto :downloadsqlite 
:downloadsqlite 
(
    curl -o packages\sqlite3\src\sqlite3.zip https://www.sqlite.org/2022/sqlite-amalgamation-3400100.zip
    tar -xf packages\sqlite3\src\sqlite3.zip 
    move "sqlite-amalgamation-3400100\shell.c" packages\sqlite3\src\shell.c
    move "sqlite-amalgamation-3400100\sqlite3.c" packages\sqlite3\src\sqlite3.c
    move "sqlite-amalgamation-3400100\sqlite3.h" packages\sqlite3\src\sqlite3.h
    del /S /Q "sqlite-amalgamation-3400100" && rmdir "sqlite-amalgamation-3400100"
    del /S /Q packages\sqlite3\src\sqlite3.zip 
)
:sqlitedownloaded
(
    echo sqlite3 package is downloaded 
)

echo Building C/C++ external libraries  
gcc packages\sqlite3\src\sqlite3.c packages\sqlite3\src\shell.c -lm -o packages\sqlite3\bin\sqlite3.exe 
echo C/C++ external libraries are built   

echo Building C/C++ projects 
if not exist extensions/SqlViewerDatabase/cpp/bin mkdir "extensions/SqlViewerDatabase/cpp/bin" 
if not exist extensions/SqlViewerConfig/cpp/bin mkdir "extensions/SqlViewerConfig/cpp/bin" 
if not exist extensions/SqlViewerNetwork/cpp/bin mkdir "extensions/SqlViewerNetwork/cpp/bin" 

gcc -c -BUILD_MY_DLL extensions/SqlViewerDatabase/cpp/src/SqlViewerDatabase.c packages/sqlite3/src/sqlite3.c
gcc -shared -o SqlViewerDatabase.dll SqlViewerDatabase.o sqlite3.o -Wl,--out-implib,SqlViewerDatabase.a
g++ -c extensions/SqlViewerConfig/cpp/src/*.cpp
g++ -o SqlViewerConfig.exe SqlViewerConfig.o -L. -lSqlViewerDatabase 

move SqlViewerDatabase.dll extensions/SqlViewerDatabase/cpp/bin/SqlViewerDatabase.dll 
move SqlViewerDatabase.o extensions/SqlViewerDatabase/cpp/bin/SqlViewerDatabase.o
move SqlViewerDatabase.a extensions/SqlViewerDatabase/cpp/bin/SqlViewerDatabase.a
move sqlite3.o extensions/SqlViewerDatabase/cpp/bin/sqlite3.o
copy extensions\SqlViewerDatabase\cpp\bin\SqlViewerDatabase.dll "extensions/SqlViewerConfig/cpp/bin"
copy extensions\SqlViewerDatabase\cpp\bin\SqlViewerDatabase.o "extensions/SqlViewerConfig/cpp/bin"
copy extensions\SqlViewerDatabase\cpp\bin\SqlViewerDatabase.a "extensions/SqlViewerConfig/cpp/bin"
copy extensions\SqlViewerDatabase\cpp\bin\sqlite3.o "extensions/SqlViewerConfig/cpp/bin"
move SqlViewerConfig.exe extensions/SqlViewerConfig/cpp/bin/SqlViewerConfig.exe 
move SqlViewerConfig.o extensions/SqlViewerConfig/cpp/bin/SqlViewerConfig.o

g++ extensions/SqlViewerNetwork/cpp/src/*.cpp -o extensions/SqlViewerNetwork/cpp/bin/SqlViewerNetworkCpp.exe 
gcc extensions\SqlViewerDatabase\cpp\src\*.c packages\sqlite3\src\sqlite3.c -shared -o extensions\SqlViewerDatabase\cpp\bin\SqlViewerDatabase.dll 
move extensions\SqlViewerDatabase\cpp\bin\SqlViewerDatabase.dll extensions\SqlViewerConfig\cpp\bin\SqlViewerDatabase.dll
echo Building C/C++ projects is finished  

echo Initialization of databases and the project's file system
rem C# version of SqlViewerConfig
rem dotnet run --project ./extensions/SqlViewerConfig/cs/SqlViewerConfig.csproj 
rem C++ version of SqlViewerConfig
"extensions/SqlViewerConfig/cpp/bin/SqlViewerConfig.exe"
echo Initialization of databases and the project's file system is finished 

echo Configuration is finished  

echo Run the application using 'run.cmd' command
