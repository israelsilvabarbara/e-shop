FROM mcr.microsoft.com/dotnet/sdk:9.0 AS shared-events
WORKDIR /src
COPY . ./
RUN dotnet restore
RUN dotnet build -c Release -o /build