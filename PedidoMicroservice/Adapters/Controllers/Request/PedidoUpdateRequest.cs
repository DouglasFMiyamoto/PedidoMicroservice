using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PedidoMicroservice.Adapters.Controllers.Request
{
    public class PedidoUpdateRequest
    {
        [FromRoute]
        [BindFrom("id")] 
        public int Id { get; set; }
        public int Status { get; set; }
    }
}
