FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Shared.Middleware/Shared.Middleware.csproj ./Shared.Middleware/
RUN dotnet restore "./Shared.Middleware/Shared.Middleware.csproj"

COPY Shared.Events/Shared.Events.csproj ./Shared.Events/
RUN dotnet restore "./Shared.Events/Shared.Events.csproj"

COPY Shared.EventBridge/Shared.EventBridge.csproj ./Shared.EventBridge/
RUN dotnet restore "./Shared.EventBridge/Shared.EventBridge.csproj"

# Copy Identity.API source code
COPY Identity.API/Identity.API.csproj Identity.API/
RUN dotnet restore Identity.API/Identity.API.csproj

# Copy Identity.KeyGen.Service source code
COPY Identity.KeyGen.Service/Identity.KeyGen.Service.csproj Identity.KeyGen.Service/
RUN dotnet restore Identity.KeyGen.Service/Identity.KeyGen.Service.csproj



# Build both projects
COPY Shared.Middleware/ ./Shared.Middleware/
COPY Shared.Events/ ./Shared.Events/
COPY Shared.EventBridge/ ./Shared.EventBridge/
COPY Identity.API/ ./Identity.API/
COPY Identity.KeyGen.Service/ ./Identity.KeyGen.Service/
RUN dotnet publish Identity.API/Identity.API.csproj -c $BUILD_CONFIGURATION -o /app
RUN dotnet publish Identity.KeyGen.Service/Identity.KeyGen.Service.csproj -c $BUILD_CONFIGURATION -o /keygen

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app


COPY --from=build /app .
COPY --from=build /keygen /keygen

# Copy the .env file into the container
COPY .env /src/.env


# Install cron
RUN apt-get update && apt-get install -y cron
# Copy the cron job and entrypoint script
COPY Identity.KeyGen.Service/cronjob /etc/cron.d/identity-keygen-cron
COPY Identity.API/entrypoint.sh /usr/local/bin/

# Set permissions for the cron job and entrypoint script
RUN chmod 0644 /etc/cron.d/identity-keygen-cron && \
    chmod +x /usr/local/bin/entrypoint.sh && \
    crontab /etc/cron.d/identity-keygen-cron

# Run the cron job
ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]
#ENTRYPOINT [ "dotnet", "Identity.API.dll" ]
