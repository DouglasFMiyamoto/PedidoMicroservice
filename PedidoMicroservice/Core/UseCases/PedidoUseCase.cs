using PedidoMicroservice.Core.Ports;
using PedidoMicroservice.Core.Entities;
using PedidoMicroservice.Adapters.Controllers.Request;
using PedidoMicroservice.Adapters.Controllers.Response;

namespace PedidoMicroservice.Core.UseCases
{
    public class PedidoUseCase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ISqsService _sqsService;

        public PedidoUseCase(IPedidoRepository pedidoRepository, ISqsService sqsService)
        {
            _pedidoRepository = pedidoRepository;
            _sqsService = sqsService;   
        }

        public async Task<PedidoResponse> CriarPedido(PedidoCreateRequest req)
        {
            var itens = new List<PedidoItem>();
            foreach (var item in req.Itens)
            {
                var pedidoItem = new PedidoItem();
                pedidoItem.Customizacao = item.Customizacao;
                pedidoItem.DataCriacao = DateTime.Now;
                pedidoItem.Quantidade = item.Quantidade;
                pedidoItem.NomeProduto = item.NomeProduto;
                pedidoItem.ProdutoId = item.ProdutoId;
                pedidoItem.Valor = item.Valor;

                itens.Add(pedidoItem);
            }

            var pedido = new Pedido
            {
                Cliente = req.Cliente,
                ClienteId = req.ClienteId,
                Valor = req.Valor,
                Status = Core.Entities.Enums.PedidoStatus.Criado,
                DataCriacao = DateTime.Now.ToUniversalTime(),
                DataAtualizacao = DateTime.Now.ToUniversalTime(),
                Itens = itens
            };

            var retorno = await _pedidoRepository.AdicionarPedido(pedido);

            var mensagem = $"{{ \"id\": {pedido.Id}, \"cliente\": { pedido.Cliente }, \"status\": { pedido.Status.ToString() } }}";
            await _sqsService.EnviarMensagemAsync(mensagem);

            return new PedidoResponse
            {
                Cliente = retorno.Cliente,
                Id = retorno.Id,
                Status = retorno.Status.ToString()
            };
        }

        public async Task<List<PedidoResponse>> ObterPedidos()
        {
            var pedidos = await _pedidoRepository.ListarPedidos();
            var response = new List<PedidoResponse>();

            foreach(var pedido in pedidos)
            {
                var pedidoResponse = new PedidoResponse();

                pedidoResponse.Id = pedido.Id;
                pedidoResponse.Cliente = pedido.Cliente;
                pedidoResponse.Status = pedido.Status.ToString();

                response.Add(pedidoResponse);
            }

            return response;
        }

        public async Task<PedidoResponse> AtualizarPedido(PedidoUpdateRequest req)
        {
            var pedidoExistente = await _pedidoRepository.ObterPedidoPorId(req.Id);

            if (pedidoExistente == null)
            {
                return null;
            }

            // Atualiza os dados do pedido
            pedidoExistente.Status = (Entities.Enums.PedidoStatus)req.Status;
            pedidoExistente.DataAtualizacao = DateTime.Now.ToUniversalTime();

            var pedidoAtualizado = await _pedidoRepository.AtualizarPedido(pedidoExistente);

            return new PedidoResponse
            {
                Id = pedidoAtualizado.Id,
                Cliente = pedidoAtualizado.Cliente,
                Status = pedidoAtualizado.Status.ToString()
            };
        }
    }
}
