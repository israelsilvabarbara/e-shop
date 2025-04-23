FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Shared.Events/Shared.Events.csproj ./Shared.Events/
RUN dotnet restore "./Shared.Events/Shared.Events.csproj"

COPY Shared.EventBridge/Shared.EventBridge.csproj ./Shared.EventBridge/
RUN dotnet restore "./Shared.EventBridge/Shared.EventBridge.csproj"

COPY Logger.API/Logger.API.csproj ./Logger.API/
RUN dotnet restore "./Logger.API/Logger.API.csproj"

COPY Shared.Events/ ./Shared.Events/
COPY Shared.EventBridge/ ./Shared.EventBridge/
COPY Logger.API/ ./Logger.API/

WORKDIR /src/Logger.API
RUN dotnet build "Logger.API.csproj" -c $BUILD_CONFIGURATION -o /src/build
RUN dotnet publish "Logger.API.csproj" -c $BUILD_CONFIGURATION -o /src/publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy published files into the runtime container
COPY --from=dev /src/publish .

ENTRYPOINT ["dotnet", "Logger.API.dll"]
