using PedidoMicroservice.Core.Entities.Enums;

namespace PedidoMicroservice.Adapters.Controllers.Request
{
    public class PedidoCreateRequest
    {
        public PedidoCreateRequest()
        {
            Itens = new List<PedidoItemRequest>();
        }

        public int ClienteId { get; set; }
        public string? Cliente { get; set; }
        public ICollection<PedidoItemRequest> Itens { get; set; }
        public DateTime DataCriacao { get; set; }
        public decimal Valor { get; set; }
    }
}
