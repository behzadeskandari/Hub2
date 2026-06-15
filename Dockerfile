# Base Image - using a stable Ubuntu version
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
# Install SkiaSharp dependencies
USER root
# change to iran mirrors
RUN sed -i 's|http://archive.ubuntu.com/ubuntu/|http://ir.archive.ubuntu.com/ubuntu/|g' /etc/apt/sources.list && \
    sed -i 's|http://security.ubuntu.com/ubuntu/|http://ir.archive.ubuntu.com/ubuntu/|g' /etc/apt/sources.list

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    ca-certificates \
    curl \
    libfontconfig1 \
    libfreetype6 \
    libpng16-16 \
    libjpeg-turbo8 \
    libgif7 \
    libx11-6 \
    libxext6 \
    libxrender1 \
    libxtst6 \
    libxi6 \
    libicu70 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/endpoint/NehzatMelli.Api/NehzatMelli.Api.csproj", "src/endpoint/NehzatMelli.Api/"]
COPY ["src/core/NehzatMelli.Application/NehzatMelli.Application.csproj", "src/core/NehzatMelli.Application/"]
COPY ["src/framework/NehzatMelli.Common/NehzatMelli.Common.csproj", "src/framework/NehzatMelli.Common/"]
COPY ["src/infrastructure/NehzatMelli.Proxy/NehzatMelli.Proxy.csproj", "src/infrastructure/NehzatMelli.Proxy/"]
COPY ["src/core/NehzatMelli.Contract/NehzatMelli.Contract.csproj", "src/core/NehzatMelli.Contract/"]
COPY ["src/core/NehzatMelli.Domain/NehzatMelli.Domain.csproj", "src/core/NehzatMelli.Domain/"]
COPY ["src/infrastructure/NehzatMelli.Persistance/NehzatMelli.Persistance.csproj", "src/infrastructure/NehzatMelli.Persistance/"]
RUN dotnet restore "./src/endpoint/NehzatMelli.Api/NehzatMelli.Api.csproj"
COPY . .
WORKDIR "/src/src/endpoint/NehzatMelli.Api"
RUN dotnet build "./NehzatMelli.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NehzatMelli.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "NehzatMelli.Api.dll"]