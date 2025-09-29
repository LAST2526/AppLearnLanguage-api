# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5149


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Last02.Api/Last02.Api.csproj", "Last02.Api/"]
COPY ["Last02.Commons/Last02.Commons.csproj", "Last02.Commons/"]
COPY ["Last02.Data/Last02.Data.csproj", "Last02.Data/"]
COPY ["Last02.Models/Last02.Models.csproj", "Last02.Models/"]
COPY ["Last02.Services/Last02.Services.csproj", "Last02.Services/"]

RUN dotnet restore "Last02.Api/Last02.Api.csproj"

COPY . .

WORKDIR "/src/Last02.Api"
RUN dotnet build "Last02.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Last02.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Last02.Api.dll"]