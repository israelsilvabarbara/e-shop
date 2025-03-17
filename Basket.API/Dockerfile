FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Basket.API.csproj .
RUN dotnet restore "Basket.API.csproj"
COPY . .
RUN dotnet build   "Basket.API.csproj" -c $BUILD_CONFIGURATION -o /src/build
RUN dotnet publish "Basket.API.csproj" -c $BUILD_CONFIGURATION -o /src/publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=dev /src/publish .

ENTRYPOINT ["dotnet", "Catalog.API.dll"]
