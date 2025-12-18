# =========================
# Imagen base (runtime)
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# =========================
# Imagen de build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos los .csproj de todos los proyectos
COPY ProyectoIdeasApi.API/ProyectoIdeasApi.API.csproj ProyectoIdeasApi.API/
COPY ProyectoIdeasApi.CONTRACT/ProyectoIdeasApi.CONTRACT.csproj ProyectoIdeasApi.CONTRACT/
COPY ProyectoIdeasApi.INFRASTRUCTURE/ProyectoIdeasApi.INFRASTRUCTURE.csproj ProyectoIdeasApi.INFRASTRUCTURE/
COPY ProyectoIdeasApi.INTERFACES/ProyectoIdeasApi.INTERFACES.csproj ProyectoIdeasApi.INTERFACES/
COPY ProyectoIdeasApi.MODEL/ProyectoIdeasApi.MODEL.csproj ProyectoIdeasApi.MODEL/
COPY ProyectoIdeasApi.SERVICES/ProyectoIdeasApi.SERVICES.csproj ProyectoIdeasApi.SERVICES/
COPY ProyectoIdeasApi.LOG/ProyectoIdeasApi.LOG.csproj ProyectoIdeasApi.LOG/
COPY ProyectoIdeasApi.App.ErrorHandlingMiddleware/ProyectoIdeasApi.App.ErrorHandlingMiddleware.csproj ProyectoIdeasApi.App.ErrorHandlingMiddleware/

# Restauramos dependencias
RUN dotnet restore ProyectoIdeasApi.API/ProyectoIdeasApi.API.csproj

# Copiamos el resto del código
COPY . .

# Compilamos en Release
WORKDIR /src/ProyectoIdeasApi.API
RUN dotnet build -c Release -o /app/build

# =========================
# Publicación
# =========================
FROM build AS publish
RUN dotnet publish ProyectoIdeasApi.API.csproj -c Release -o /app/publish

# =========================
# Imagen final
# =========================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProyectoIdeasApi.API.dll"]
