# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SqlViewer.Etl.Worker/SqlViewer.Etl.Worker.csproj", "src/SqlViewer.Etl.Worker/"]
COPY ["src/SqlViewer.Shared/SqlViewer.Shared.csproj", "src/SqlViewer.Shared/"]
COPY ["src/SqlViewer.Etl.Core/SqlViewer.Etl.Core.csproj", "src/SqlViewer.Etl.Core/"]
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages dotnet restore "./src/SqlViewer.Etl.Worker/SqlViewer.Etl.Worker.csproj"
COPY . .
WORKDIR "/src/src/SqlViewer.Etl.Worker"
RUN dotnet build "./SqlViewer.Etl.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SqlViewer.Etl.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SqlViewer.Etl.Worker.dll"]