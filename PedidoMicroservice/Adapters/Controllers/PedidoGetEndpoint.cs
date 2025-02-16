using FastEndpoints;
using PedidoMicroservice.Core.UseCases;
using PedidoMicroservice.Adapters.Controllers.Response;

namespace PedidoMicroservice.Adapters.Controllers
{
    public class PedidoGetAllEndpoint : EndpointWithoutRequest<List<PedidoResponse>>
    {
        private readonly PedidoUseCase _pedidoUseCase;

        public PedidoGetAllEndpoint(PedidoUseCase pedidoUseCase)
        {
            _pedidoUseCase = pedidoUseCase;
        }

        public override void Configure()
        {
            Get("/pedidos");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var pedidos = await _pedidoUseCase.ObterPedidos();
            await SendAsync(pedidos);
        }
    }
}
