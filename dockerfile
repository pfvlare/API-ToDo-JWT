# Etapa 1: build do projeto
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia tudo para dentro do container
COPY . ./

# Publica o projeto para produção
RUN dotnet publish TodoApiNovo.csproj -c Release -o out

# Etapa 2: imagem final para execução
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copia a saída da build
COPY --from=build /app/out ./

# Porta exposta (Render vai redirecionar a variável PORT)
EXPOSE 10000

# Comando de inicialização
ENTRYPOINT ["dotnet", "TodoApiNovo.dll"]