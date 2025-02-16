using Moq;
using Xunit;
using PedidoMicroservice.Core.Ports;
using PedidoMicroservice.Core.Entities;
using PedidoMicroservice.Adapters.Controllers.Request;
using PedidoMicroservice.Core.UseCases;
using PedidoMicroservice.Core.Entities.Enums;
using Assert = Xunit.Assert;

public class PedidoUseCaseTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<ISqsService> _sqsServiceMock;
    private readonly PedidoUseCase _pedidoUseCase;

    public PedidoUseCaseTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _sqsServiceMock = new Mock<ISqsService>();
        _pedidoUseCase = new PedidoUseCase(_pedidoRepositoryMock.Object, _sqsServiceMock.Object);
    }

    [Fact]
    public async Task CriarPedido_DeveAdicionarPedidoERetornarResponse()
    {
        // Arrange
        var pedidoRequest = new PedidoCreateRequest
        {
            Cliente = "João",
            ClienteId = 1,
            Valor = 100,
            Itens = new List<PedidoItemRequest>
            {
                new PedidoItemRequest { NomeProduto = "Produto 1", Quantidade = 2, Valor = 50 }
            }
        };

        var pedidoCriado = new Pedido
        {
            Id = 1,
            Cliente = "João",
            ClienteId = 1,
            Valor = 100,
            Status = PedidoStatus.Criado,
            Itens = new List<PedidoItem>()
        };

        _pedidoRepositoryMock.Setup(r => r.AdicionarPedido(It.IsAny<Pedido>())).ReturnsAsync(pedidoCriado);

        // Act
        var result = await _pedidoUseCase.CriarPedido(pedidoRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pedidoCriado.Cliente, result.Cliente);
        Assert.Equal(pedidoCriado.Id, result.Id);
        Assert.Equal(pedidoCriado.Status.ToString(), result.Status);
        _pedidoRepositoryMock.Verify(r => r.AdicionarPedido(It.IsAny<Pedido>()), Times.Once);
        _sqsServiceMock.Verify(s => s.EnviarMensagemAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ObterPedidos_DeveRetornarListaDePedidos()
    {
        // Arrange
        var pedidos = new List<Pedido>
        {
            new Pedido { Id = 1, Cliente = "João", Status = PedidoStatus.Criado },
            new Pedido { Id = 2, Cliente = "Maria", Status = PedidoStatus.Finalizado }
        };

        _pedidoRepositoryMock.Setup(r => r.ListarPedidos()).ReturnsAsync(pedidos);

        // Act
        var result = await _pedidoUseCase.ObterPedidos();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("João", result[0].Cliente);
        Assert.Equal("Maria", result[1].Cliente);
        _pedidoRepositoryMock.Verify(r => r.ListarPedidos(), Times.Once);
    }

    [Fact]
    public async Task AtualizarPedido_DeveRetornarPedidoAtualizado()
    {
        // Arrange
        var pedidoExistente = new Pedido { Id = 1, Cliente = "João", Status = PedidoStatus.Criado };
        var pedidoAtualizado = new Pedido { Id = 1, Cliente = "João", Status = PedidoStatus.Finalizado };

        _pedidoRepositoryMock.Setup(r => r.ObterPedidoPorId(1)).ReturnsAsync(pedidoExistente);
        _pedidoRepositoryMock.Setup(r => r.AtualizarPedido(It.IsAny<Pedido>())).ReturnsAsync(pedidoAtualizado);

        var updateRequest = new PedidoUpdateRequest { Id = 1, Status = (int)PedidoStatus.Finalizado };

        // Act
        var result = await _pedidoUseCase.AtualizarPedido(updateRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pedidoAtualizado.Id, result.Id);
        Assert.Equal(pedidoAtualizado.Cliente, result.Cliente);
        Assert.Equal(pedidoAtualizado.Status.ToString(), result.Status);
        _pedidoRepositoryMock.Verify(r => r.ObterPedidoPorId(1), Times.Once);
        _pedidoRepositoryMock.Verify(r => r.AtualizarPedido(It.IsAny<Pedido>()), Times.Once);
    }
}