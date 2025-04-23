FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Shared.Events/Shared.Events.csproj ./Shared.Events/
RUN dotnet restore "./Shared.Events/Shared.Events.csproj"

COPY Shared.EventBridge/Shared.EventBridge.csproj ./Shared.EventBridge/
RUN dotnet restore "./Shared.EventBridge/Shared.EventBridge.csproj"

COPY Basket.API/Basket.API.csproj ./Basket.API/
RUN dotnet restore "./Basket.API/Basket.API.csproj"

COPY Shared.Events/ ./Shared.Events/
COPY Shared.EventBridge/ ./Shared.EventBridge/
COPY Basket.API/ ./Basket.API/

WORKDIR /src/Basket.API
RUN dotnet build   "Basket.API.csproj" -c $BUILD_CONFIGURATION -o /src/build
RUN dotnet publish "Basket.API.csproj" -c $BUILD_CONFIGURATION -o /src/publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=dev /src/publish .

ENTRYPOINT ["dotnet", "Basket.API.dll"]
