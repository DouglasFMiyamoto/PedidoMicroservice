namespace PedidoMicroservice.Core.Entities
{
    public class PedidoItem
    {
        public Guid Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public string? Customizacao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
        public virtual Pedido? Pedido { get; set; }
    }
}
