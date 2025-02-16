using PedidoMicroservice.Core.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace PedidoMicroservice.Core.Entities
{
    public class Pedido
    {
        public Pedido()
        {
            Itens = new HashSet<PedidoItem>();
        }

        public Pedido(int clienteId, string cliente, PedidoStatus status, ICollection<PedidoItem> itens, DateTime dataCriacao, decimal valor)
        {
            ClienteId = clienteId;
            Cliente = cliente;
            Status = status;
            Itens = itens ?? new List<PedidoItem>();
            DataCriacao = dataCriacao;
            Valor = valor;
        }

        [Key]
        [Required]
        public int Id { get; set; }        
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public string Cliente { get; set; }
        [Required]
        public PedidoStatus Status { get; set; }
        [Required]
        public ICollection<PedidoItem> Itens { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; }
        [Required]
        public DateTime DataAtualizacao { get; set; }
        [Required]
        public decimal Valor { get; set; }
    }
}
