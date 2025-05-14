FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Shared.Keycloak/Shared.Keycloak.csproj ./Shared.Keycloak/
RUN dotnet restore "./Shared.Keycloak/Shared.Keycloak.csproj"

COPY Shared.Events/Shared.Events.csproj ./Shared.Events/
RUN dotnet restore "./Shared.Events/Shared.Events.csproj"

COPY Shared.EventBridge/Shared.EventBridge.csproj ./Shared.EventBridge/
RUN dotnet restore "./Shared.EventBridge/Shared.EventBridge.csproj"

COPY Inventory.API/Inventory.API.csproj ./Inventory.API/
RUN dotnet restore "./Inventory.API/Inventory.API.csproj"

COPY Shared.Keycloak/ ./Shared.Keycloak/
COPY Shared.Events/ ./Shared.Events/
COPY Shared.EventBridge/ ./Shared.EventBridge/
COPY Inventory.API/ ./Inventory.API/

WORKDIR /src/Inventory.API
RUN dotnet build   "Inventory.API.csproj" -c $BUILD_CONFIGURATION -o /src/build
RUN dotnet publish "Inventory.API.csproj" -c $BUILD_CONFIGURATION -o /src/publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=dev /src/publish .

ENTRYPOINT ["dotnet", "Inventory.API.dll"]
