#!/bin/bash

# Source the .env file to export variables
if [ -f "/src/.env" ]; then
  # Export variables from the .env file
  export $(grep -v '^#' /src/.env | xargs)

  # Write the environment variables to /etc/environment for cron
  printenv | sed 's/^\(.*\)$/export \1/g' > /etc/environment
else
  echo "WARNING: .env file not found at /src/.env"
fi

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