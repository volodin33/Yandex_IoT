FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER root

RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/* \
    
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DeviceRegistry/DeviceRegistry.csproj", "DeviceRegistry/"]
COPY ["SmarthomeCore/SmarthomeCore.csproj", "SmarthomeCore/"]
RUN dotnet restore "DeviceRegistry/DeviceRegistry.csproj"
COPY . .
WORKDIR "/src/DeviceRegistry"
RUN dotnet build "./DeviceRegistry.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeviceRegistry.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceRegistry.dll"]
