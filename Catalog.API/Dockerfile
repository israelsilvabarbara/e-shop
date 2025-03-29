# Stage 1: Build Shared.Events
FROM shared-events:latest AS shared-events-stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Catalog.API.csproj .
RUN dotnet restore "Catalog.API.csproj"
COPY . .

# Copy Shared.Events build output
COPY --from=shared-events-stage /build ./Shared.Events/

RUN dotnet build   "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /src/build
RUN dotnet publish "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /src/publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=dev /src/publish .

ENTRYPOINT ["dotnet", "Catalog.API.dll"]
