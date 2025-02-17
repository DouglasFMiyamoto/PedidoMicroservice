FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Definir a imagem base para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar apenas o .csproj do projeto principal
COPY ["PedidoMicroservice.csproj", "./"]

# Restaurar dependências do projeto principal
RUN dotnet restore "PedidoMicroservice.csproj"

# Copiar apenas os arquivos do projeto principal (excluindo a pasta de testes)
COPY PedidoMicroservice/. . 

WORKDIR "/src"
RUN dotnet build "PedidoMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PedidoMicroservice.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

# Copiar o conteúdo do publish para o container final
COPY --from=publish /app/publish .

# Definir variável de ambiente para o LocalStack
ENV AWS_SERVICE_URL=http://localstack:4566

ENTRYPOINT ["dotnet", "PedidoMicroservice.dll"]
