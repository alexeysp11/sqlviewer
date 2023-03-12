@echo off 

echo Configuration is started 

echo Restoring C# projects 
dotnet restore ./dbconnections/SqlViewerDatabase/SqlViewerDatabase.csproj
dotnet restore ./extensions/SqlViewerConfig/SqlViewerConfig.csproj
dotnet restore ./wpf/SqlViewer.csproj
dotnet restore ./aspnet/SqlViewerAspNet.csproj
echo All C# projects are restored 

echo Initialization of databases and the project's file system
dotnet run --project ./extensions/SqlViewerConfig/SqlViewerConfig.csproj 
echo Initialization of databases and the project's file system is finished 

echo Configuration is finished  

echo Run the application using 'run.cmd' command
