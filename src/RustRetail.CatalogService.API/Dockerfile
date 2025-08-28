# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8084
EXPOSE 8085


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RustRetail.CatalogService.API/RustRetail.CatalogService.API.csproj", "src/RustRetail.CatalogService.API/"]
COPY ["src/RustRetail.CatalogService.Infrastructure/RustRetail.CatalogService.Infrastructure.csproj", "src/RustRetail.CatalogService.Infrastructure/"]
COPY ["src/RustRetail.CatalogService.Application/RustRetail.CatalogService.Application.csproj", "src/RustRetail.CatalogService.Application/"]
COPY ["src/RustRetail.CatalogService.Contracts/RustRetail.CatalogService.Contracts.csproj", "src/RustRetail.CatalogService.Contracts/"]
COPY ["src/RustRetail.CatalogService.Domain/RustRetail.CatalogService.Domain.csproj", "src/RustRetail.CatalogService.Domain/"]
COPY ["src/RustRetail.CatalogService.Persistence/RustRetail.CatalogService.Persistence.csproj", "src/RustRetail.CatalogService.Persistence/"]
RUN dotnet restore "./src/RustRetail.CatalogService.API/RustRetail.CatalogService.API.csproj"
COPY . .
WORKDIR "/src/src/RustRetail.CatalogService.API"
RUN dotnet build "./RustRetail.CatalogService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RustRetail.CatalogService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RustRetail.CatalogService.API.dll"]