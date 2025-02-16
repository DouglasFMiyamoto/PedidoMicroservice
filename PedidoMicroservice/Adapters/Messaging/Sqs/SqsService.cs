using Amazon.SQS;
using Amazon.SQS.Model;
using PedidoMicroservice.Core.Ports;

namespace PedidoMicroservice.Adapters.Messaging.Sqs
{
    public class SqsService : ISqsService
    {
        private readonly IAmazonSQS _sqsClient;
        private string _queueUrl;
        private const string QueueName = "pedido-criado";  // Nome da fila que você quer criar ou acessar

        public SqsService(IConfiguration configuration)
        {
            _sqsClient = new AmazonSQSClient(
                "test", "test",  // Essas credenciais são apenas para o LocalStack, pode manter assim
                new AmazonSQSConfig { ServiceURL = "http://localstack:4566" }
            );
            _queueUrl = $"http://localstack:4566/000000000000/{QueueName}";  // URL da fila
        }

        public async Task EnviarMensagemAsync(string mensagem)
        {
            // Verifica se a fila já existe, se não, cria a fila
            await CriarFilaSeNaoExistirAsync();

            var request = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = mensagem
            };

            await _sqsClient.SendMessageAsync(request);
            Console.WriteLine($"Mensagem enviada para SQS: {mensagem}");
        }

        private async Task CriarFilaSeNaoExistirAsync()
        {
            try
            {
                // Tentando obter a URL da fila (isso vai lançar uma exceção se a fila não existir)
                var getQueueUrlRequest = new GetQueueUrlRequest
                {
                    QueueName = QueueName
                };
                var response = await _sqsClient.GetQueueUrlAsync(getQueueUrlRequest);
                _queueUrl = response.QueueUrl;  // Atualiza o URL da fila
            }
            catch (QueueDoesNotExistException)
            {
                // Se a fila não existir, criamos a fila
                Console.WriteLine($"Fila '{QueueName}' não existe. Criando fila...");
                var createQueueRequest = new CreateQueueRequest
                {
                    QueueName = QueueName
                };
                var createResponse = await _sqsClient.CreateQueueAsync(createQueueRequest);
                _queueUrl = createResponse.QueueUrl;  
                Console.WriteLine($"Fila '{QueueName}' criada com sucesso.");
            }
        }
    }
}