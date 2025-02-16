using FastEndpoints;
using PedidoMicroservice.Core.UseCases;
using PedidoMicroservice.Adapters.Controllers.Request;
using PedidoMicroservice.Adapters.Controllers.Response;

namespace PedidoMicroservice.Adapters.Controllers
{
    public class PedidoUpdateEndpoint : Endpoint<PedidoUpdateRequest, PedidoResponse>
    {
        private readonly PedidoUseCase _pedidoUseCase;

        public PedidoUpdateEndpoint(PedidoUseCase pedidoUseCase)
        {
            _pedidoUseCase = pedidoUseCase;
        }

        public override void Configure()
        {
            Put("/pedidos");
            AllowAnonymous();
        }

        public override async Task HandleAsync(PedidoUpdateRequest req, CancellationToken ct)
        {
            var resposta = await _pedidoUseCase.AtualizarPedido(req);
            await SendAsync(resposta);
        }
    }
}
