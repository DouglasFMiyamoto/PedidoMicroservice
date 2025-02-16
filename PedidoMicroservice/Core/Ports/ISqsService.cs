namespace PedidoMicroservice.Core.Ports
{
    public interface ISqsService
    {
        Task EnviarMensagemAsync(string mensagem);
    }
}
