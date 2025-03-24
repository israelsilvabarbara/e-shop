FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Inventory.API.csproj .
RUN dotnet restore "Inventory.API.csproj"
COPY . .
RUN dotnet build   "Inventory.API.csproj" -c $BUILD_CONFIGURATION -o /src/build
RUN dotnet publish "Inventory.API.csproj" -c $BUILD_CONFIGURATION -o /src/publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=dev /src/publish .

ENTRYPOINT ["dotnet", "Inventory.API.dll"]
