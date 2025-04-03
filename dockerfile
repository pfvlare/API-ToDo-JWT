# Etapa 1: build do projeto
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia tudo
COPY . ./

# ✅ Especifica qual projeto compilar
RUN dotnet publish TodoApiNovo.csproj -c Release -o out

# Etapa 2: imagem final para execução
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 80

ENTRYPOINT ["dotnet", "TodoApiNovo.dll"]
