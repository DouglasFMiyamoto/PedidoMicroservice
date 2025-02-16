using PedidoMicroservice.Core.UseCases;
using FastEndpoints;
using PedidoMicroservice.Adapters.Controllers.Request;
using PedidoMicroservice.Adapters.Controllers.Response;

namespace PedidoMicroservice.Adapters.Controllers
{
    public class PedidoCreateEndpoints : Endpoint<PedidoCreateRequest, PedidoResponse>
    {
        private readonly PedidoUseCase _pedidoUseCase;

        public PedidoCreateEndpoints(PedidoUseCase pedidoUseCase)
        {
            _pedidoUseCase = pedidoUseCase;
        }

        public override void Configure()
        {
            Post("/pedidos");
            AllowAnonymous();
        }

        public override async Task HandleAsync(PedidoCreateRequest req, CancellationToken ct)
        {
            var response = await _pedidoUseCase.CriarPedido(req);
            await SendAsync(response);
        }
    }
}
