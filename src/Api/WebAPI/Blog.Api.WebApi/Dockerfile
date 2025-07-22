# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/WebAPI/Blog.Api.WebApi/Blog.Api.WebApi.csproj", "src/Api/WebAPI/Blog.Api.WebApi/"]
COPY ["src/Api/Infrastructure/Blog.Infrastructure.Persistence/Blog.Infrastructure.Persistence.csproj", "src/Api/Infrastructure/Blog.Infrastructure.Persistence/"]
COPY ["src/Common/Blog.Common/Blog.Common.csproj", "src/Common/Blog.Common/"]
COPY ["src/Api/Core/Blog.Api.Application/Blog.Api.Application.csproj", "src/Api/Core/Blog.Api.Application/"]
COPY ["src/Api/Infrastructure/Blog.Infrastructure.Security/Blog.Infrastructure.Security.csproj", "src/Api/Infrastructure/Blog.Infrastructure.Security/"]
COPY ["src/Api/Infrastructure/Blog.Infrastructure.Utilities/Blog.Infrastructure.Utilities.csproj", "src/Api/Infrastructure/Blog.Infrastructure.Utilities/"]
COPY ["src/Api/Core/Blog.Api.Domain/Blog.Api.Domain.csproj", "src/Api/Core/Blog.Api.Domain/"]
RUN dotnet restore "./src/Api/WebAPI/Blog.Api.WebApi/Blog.Api.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Api/WebAPI/Blog.Api.WebApi"
RUN dotnet build "./Blog.Api.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Blog.Api.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.Api.WebApi.dll"]