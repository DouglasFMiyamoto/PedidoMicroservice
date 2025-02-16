namespace PedidoMicroservice.Adapters.Controllers.Request
{
    public class PedidoItemRequest
    {
        public int ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public string? Customizacao { get; set; }
        public decimal Valor { get; set; }
    }
}
