# Microserviço de Pedido

## Descrição
O microserviço de Pedido é responsável por criar pedidos, registrar essas informações em um banco de dados PostgreSQL e coordenar a troca de mensagens entre outros microserviços via Amazon SQS para garantir o fluxo de processamento do pedido.

## Fluxo do Pedido
1. **Criação do Pedido**: Um novo pedido é criado e salvo no banco de dados PostgreSQL.
2. **Publicação na Fila `pedido-criado`**: Uma mensagem é publicada na fila Amazon SQS `pedido-criado`.
3. **Processamento de Pagamento**: O serviço de pagamento consome a mensagem e processa o pagamento.
4. **Publicação na Fila `pedido-pago`**: Após a confirmação do pagamento, uma nova mensagem é enviada para a fila `pedido-pago`.
5. **Início da Produção**: O serviço de produção consome a mensagem para iniciar a produção do pedido.
6. **Atualização do Pedido**: Quando a produção é concluída ou o status do pedido é alterado, uma nova mensagem é publicada na fila `pedido-atualizado`.
7. **Consumo e Atualização do Status**: O microserviço de Pedido consome a mensagem da fila `pedido-atualizado` e atualiza o status do pedido no banco de dados.

## Tecnologias Utilizadas
- **.NET 8** (C#)
- **PostgreSQL** (Banco de dados)
- **Amazon SQS** (Mensageria)
- **Docker** (Containerização)
- **LocalStack** (Simulação do AWS SQS em ambiente local para testes)

## Como Executar o Microserviço
### 1. Configurar Dependências
Certifique-se de que você tem os seguintes serviços configurados e em execução:
- **PostgreSQL**: Um banco de dados PostgreSQL ativo.
- **LocalStack (opcional para testes locais)**: Para simular o SQS.

### 2. Configurar Variáveis de Ambiente
Defina as variáveis necessárias no ambiente para conexão com o PostgreSQL e o SQS.

Exemplo de configuração no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=pedidos;Username=usuario;Password=senha"
  },
  "AWS": {
    "SQS": {
      "PedidoCriadoQueueUrl": "http://localhost:4566/000000000000/pedido-criado",
      "PedidoPagoQueueUrl": "http://localhost:4566/000000000000/pedido-pago",
      "PedidoAtualizadoQueueUrl": "http://localhost:4566/000000000000/pedido-atualizado"
    }
  }
}
```

### 3. Executar a Aplicação
Com o Docker:
```sh
docker-compose up --build
```
Ou localmente:
```sh
dotnet run
```

## Testes
Este microserviço conta com testes de unidade para garantir a qualidade do código. Para rodar os testes:
```sh
dotnet test
```
