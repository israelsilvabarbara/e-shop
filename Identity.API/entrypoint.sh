#!/bin/bash

# Start the cron service
echo "Starting cron service..."
service cron start


# Run Identity.KeyGen.Service
echo "Starting Identity.KeyGen.Service..."
dotnet /keygen/Identity.KeyGen.Service.dll

# Run Identity.API
echo "Starting Identity.API..."
exec dotnet /app/Identity.API.dll
