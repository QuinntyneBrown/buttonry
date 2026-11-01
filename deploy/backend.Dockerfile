# Builds the three backend services from one restore. Build context: backend.
#   docker build -f ../deploy/backend.Dockerfile --target api .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Directory.Build.props global.json Buttonry.sln ./
COPY src/Buttonry.Domain/Buttonry.Domain.csproj src/Buttonry.Domain/
COPY src/Buttonry.Application/Buttonry.Application.csproj src/Buttonry.Application/
COPY src/Buttonry.Infrastructure/Buttonry.Infrastructure.csproj src/Buttonry.Infrastructure/
COPY src/Buttonry.Api/Buttonry.Api.csproj src/Buttonry.Api/
COPY src/Buttonry.Worker/Buttonry.Worker.csproj src/Buttonry.Worker/
COPY src/Buttonry.Migrator/Buttonry.Migrator.csproj src/Buttonry.Migrator/
RUN dotnet restore src/Buttonry.Api/Buttonry.Api.csproj \
 && dotnet restore src/Buttonry.Worker/Buttonry.Worker.csproj \
 && dotnet restore src/Buttonry.Migrator/Buttonry.Migrator.csproj
COPY . .
RUN dotnet publish src/Buttonry.Api/Buttonry.Api.csproj -c Release --no-restore -o /out/api \
 && dotnet publish src/Buttonry.Worker/Buttonry.Worker.csproj -c Release --no-restore -o /out/worker \
 && dotnet publish src/Buttonry.Migrator/Buttonry.Migrator.csproj -c Release --no-restore -o /out/migrator

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS api
WORKDIR /app
COPY --from=build /out/api .
RUN mkdir -p /data/photos && chown app:app /data/photos
ENV ASPNETCORE_URLS=http://+:8080 DOTNET_ENVIRONMENT=Production
EXPOSE 8080
USER app
ENTRYPOINT ["dotnet", "Buttonry.Api.dll"]

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS worker
WORKDIR /app
COPY --from=build /out/worker .
RUN mkdir -p /data/photos && chown app:app /data/photos
ENV DOTNET_ENVIRONMENT=Production
USER app
ENTRYPOINT ["dotnet", "Buttonry.Worker.dll"]

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS migrator
WORKDIR /app
COPY --from=build /out/migrator .
USER app
ENTRYPOINT ["dotnet", "Buttonry.Migrator.dll"]
