@echo off 

rem Add a module for initialization of DB and project's file system, and restoring packages for the first time 

dotnet run --project ./src/SqlViewer.csproj
