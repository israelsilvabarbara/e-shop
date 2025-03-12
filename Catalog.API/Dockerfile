FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Catalog.API.csproj .
RUN dotnet restore "Catalog.API.csproj"
COPY . .

RUN dotnet publish "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /src/publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

EXPOSE 5000
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /src/publish .

ENTRYPOINT ["dotnet", "Catalog.API.dll"]
