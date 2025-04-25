#!/bin/bash

printenv > /tmp/env_debug.log
printenv > /etc/environment


# Start the cron service
echo "Starting cron service..."
service cron start


# Run Identity.KeyGen.Service
source /etc/environment
echo "Starting Identity.KeyGen.Service..."
dotnet /keygen/Identity.KeyGen.Service.dll

# Run Identity.API
echo "Starting Identity.API..."
exec dotnet /app/Identity.API.dll