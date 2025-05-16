FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


COPY Shared.Keycloak/Shared.Keycloak.csproj ./Shared.Keycloak/
RUN dotnet restore "./Shared.Keycloak/Shared.Keycloak.csproj"

COPY Shared.Events/Shared.Events.csproj ./Shared.Events/
RUN dotnet restore "./Shared.Events/Shared.Events.csproj"

COPY Shared.EventBridge/Shared.EventBridge.csproj ./Shared.EventBridge/
RUN dotnet restore "./Shared.EventBridge/Shared.EventBridge.csproj"

COPY Catalog.API/Catalog.API.csproj ./Catalog.API/
RUN dotnet restore "./Catalog.API/Catalog.API.csproj"

COPY Shared.Keycloak/ ./Shared.Keycloak/
COPY Shared.Events/ ./Shared.Events/
COPY Shared.EventBridge/ ./Shared.EventBridge/
COPY Catalog.API/ ./Catalog.API/

WORKDIR /src/Catalog.API
RUN dotnet build "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /src/build
RUN dotnet publish "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /src/publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy published files into the runtime container
COPY --from=dev /src/publish .


#RUN apt-get update && apt-get install -y curl && apt-get install -y iputils-ping

ENTRYPOINT ["dotnet", "Catalog.API.dll"]
