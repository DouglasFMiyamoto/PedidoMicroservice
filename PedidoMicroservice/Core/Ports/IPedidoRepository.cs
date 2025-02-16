using PedidoMicroservice.Core.Entities;

namespace PedidoMicroservice.Core.Ports
{
    public interface IPedidoRepository
    {
        Task<Pedido> AdicionarPedido(Pedido pedido);
        Task<List<Pedido>> ListarPedidos();
        Task<Pedido> ObterPedidoPorId(int id);
        Task<Pedido?> AtualizarPedido(Pedido pedido);
    }
}
