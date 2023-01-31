@echo off 

echo Restoring all projects 
dotnet restore ./extensions/SqlViewerDatabase/SqlViewerDatabase.csproj
dotnet restore ./extensions/SqlViewerConfig/cs/SqlViewerConfig.csproj
dotnet restore ./extensions/SqlViewerNetwork/cs/SqlViewerNetwork.csproj
dotnet restore ./src/SqlViewer.csproj

echo Initialization of databases and the project's file system
dotnet run --project ./extensions/SqlViewerConfig/cs/SqlViewerConfig.csproj
