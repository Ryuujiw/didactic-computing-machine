FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Shopify/Shopify.csproj", "Shopify/"]
RUN dotnet restore "Shopify/Shopify.csproj"
COPY . .
WORKDIR "/src/Shopify"
RUN dotnet build "Shopify.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN dotnet publish Shopify.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false \
    /p:PublishTrimmed=true \
    /p:PublishReadyToRun=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shopify.dll"]
