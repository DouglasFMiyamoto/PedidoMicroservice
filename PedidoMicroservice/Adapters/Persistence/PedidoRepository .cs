using PedidoMicroservice.Core.Entities;
using PedidoMicroservice.Core.Ports;
using Microsoft.EntityFrameworkCore;
using PedidoMicroservice.Adapters.Database.PostgreSQL;

namespace PedidoMicroservice.Adapters.Persistence
{
    public class PedidoSqlRepository : IPedidoRepository
    {
        private readonly PedidoContext _context;

        public PedidoSqlRepository(PedidoContext context)
        {
            _context = context;
        }

        public async Task<Pedido> AdicionarPedido(Pedido pedido)
        {
            // Adiciona o pedido, sem os itens ainda
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();  // Aqui o pedido.Id é gerado.

            // Agora, com o pedido.Id gerado, podemos adicionar os itens
            foreach (var item in pedido.Itens)
            {
                item.Id = Guid.NewGuid();
                item.PedidoId = pedido.Id;  // Garantimos que o PedidoId está correto
                _context.PedidoItens.Add(item);
            }

            // Agora que todos os itens estão adicionados, podemos salvar tudo
            await _context.SaveChangesAsync();

            return pedido;
        }

        // Método para listar todos os pedidos
        public async Task<List<Pedido>> ListarPedidos()
        {
            return await _context.Pedidos.Include(p => p.Itens).ToListAsync();
        }

        public async Task<Pedido> ObterPedidoPorId(int id)
        {
            return await _context.Pedidos
                            .Include(p => p.Itens)
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pedido?> AtualizarPedido(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }
    }
}
