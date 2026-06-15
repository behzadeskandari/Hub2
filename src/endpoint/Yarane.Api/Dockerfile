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
COPY ["src/endpoint/Yarane.Api/Yarane.Api.csproj", "src/endpoint/Yarane.Api/"]
COPY ["src/core/Yarane.Application/Yarane.Application.csproj", "src/core/Yarane.Application/"]
COPY ["src/framework/Yarane.Common/Yarane.Common.csproj", "src/framework/Yarane.Common/"]
COPY ["src/infrastructure/Yarane.Proxy/Yarane.Proxy.csproj", "src/infrastructure/Yarane.Proxy/"]
COPY ["src/core/Yarane.Contract/Yarane.Contract.csproj", "src/core/Yarane.Contract/"]
COPY ["src/core/Yarane.Domain/Yarane.Domain.csproj", "src/core/Yarane.Domain/"]
COPY ["src/infrastructure/Yarane.Persistance/Yarane.Persistance.csproj", "src/infrastructure/Yarane.Persistance/"]
RUN dotnet restore "./src/endpoint/Yarane.Api/Yarane.Api.csproj"
COPY . .
WORKDIR "/src/src/endpoint/Yarane.Api"
RUN dotnet build "./Yarane.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Yarane.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Yarane.Api.dll"]