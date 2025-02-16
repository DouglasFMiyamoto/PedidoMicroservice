FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Definir a imagem base para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PedidoMicroservice/PedidoMicroservice.csproj", "./"]
RUN dotnet restore "PedidoMicroservice.csproj"

COPY . . 

WORKDIR "/src"
RUN dotnet build "PedidoMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PedidoMicroservice.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Definir variável de ambiente para o LocalStack
ENV AWS_SERVICE_URL=http://localstack:4566

ENTRYPOINT ["dotnet", "PedidoMicroservice.dll"]
