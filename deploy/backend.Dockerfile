# Builds the three backend services from one restore. Build context: src/backend.
#   docker build -f ../../deploy/backend.Dockerfile --target api .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Directory.Build.props global.json Buttonry.sln ./
COPY Buttonry.Domain/Buttonry.Domain.csproj Buttonry.Domain/
COPY Buttonry.Application/Buttonry.Application.csproj Buttonry.Application/
COPY Buttonry.Infrastructure/Buttonry.Infrastructure.csproj Buttonry.Infrastructure/
COPY Buttonry.Api/Buttonry.Api.csproj Buttonry.Api/
COPY Buttonry.Worker/Buttonry.Worker.csproj Buttonry.Worker/
COPY Buttonry.Migrator/Buttonry.Migrator.csproj Buttonry.Migrator/
RUN dotnet restore Buttonry.Api/Buttonry.Api.csproj \
 && dotnet restore Buttonry.Worker/Buttonry.Worker.csproj \
 && dotnet restore Buttonry.Migrator/Buttonry.Migrator.csproj
COPY . .
RUN dotnet publish Buttonry.Api/Buttonry.Api.csproj -c Release --no-restore -o /out/api \
 && dotnet publish Buttonry.Worker/Buttonry.Worker.csproj -c Release --no-restore -o /out/worker \
 && dotnet publish Buttonry.Migrator/Buttonry.Migrator.csproj -c Release --no-restore -o /out/migrator

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
