# Usa imagem oficial do .NET SDK para buildar
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia os arquivos do projeto
COPY . ./
RUN dotnet publish -c Release -o out

# Usa imagem do runtime pra executar
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "TodoApiNovo.dll"]