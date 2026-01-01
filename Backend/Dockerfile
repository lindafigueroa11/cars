# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar csproj y restaurar
COPY Backend/Backend.csproj Backend/
RUN dotnet restore Backend/Backend.csproj

# Copiar todo y compilar
COPY . .
WORKDIR /app/Backend
RUN dotnet publish -c Release -o /out

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Render usa la variable PORT
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE 8080

ENTRYPOINT ["dotnet", "Backend.dll"]
# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar csproj y restaurar
COPY Backend/Backend.csproj Backend/
RUN dotnet restore Backend/Backend.csproj

# Copiar todo y compilar
COPY . .
WORKDIR /app/Backend
RUN dotnet publish -c Release -o /out

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Render usa la variable PORT
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE 8080

ENTRYPOINT ["dotnet", "Backend.dll"]
