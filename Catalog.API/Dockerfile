FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Shared.Events/Shared.Events.csproj ./Shared.Events/
RUN dotnet restore "./Shared.Events/Shared.Events.csproj"
COPY Shared.Events/ ./Shared.Events/

COPY Catalog.API/Catalog.API.csproj ./Catalog.API/
RUN dotnet restore "./Catalog.API/Catalog.API.csproj"
COPY Catalog.API/ ./Catalog.API/

WORKDIR /src/Catalog.API
RUN dotnet build "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /src/build
RUN dotnet publish "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /src/publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy published files into the runtime container
COPY --from=dev /src/publish .

ENTRYPOINT ["dotnet", "Catalog.API.dll"]
