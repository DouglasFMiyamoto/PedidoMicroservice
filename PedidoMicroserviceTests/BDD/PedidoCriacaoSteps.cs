using Moq;
using PedidoMicroservice.Core.Ports;
using PedidoMicroservice.Core.Entities;
using PedidoMicroservice.Adapters.Controllers.Request;
using PedidoMicroservice.Core.UseCases;
using PedidoMicroservice.Core.Entities.Enums;
using TechTalk.SpecFlow;
using PedidoMicroservice.Adapters.Controllers.Response;
using Assert = Xunit.Assert;

[Binding]
public class PedidoCriacaoSteps
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<ISqsService> _sqsServiceMock;
    private readonly PedidoUseCase _pedidoUseCase;
    private PedidoCreateRequest _pedidoRequest;
    private PedidoResponse _pedidoResponse;

    public PedidoCriacaoSteps()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _sqsServiceMock = new Mock<ISqsService>();
        _pedidoUseCase = new PedidoUseCase(_pedidoRepositoryMock.Object, _sqsServiceMock.Object);
    }

    [Given(@"um pedido válido com os seguintes itens:")]
    public void DadoUmPedidoValidoComOsSeguintesItens(Table table)
    {
        var itens = new List<PedidoItemRequest>();

        foreach (var row in table.Rows)
        {
            itens.Add(new PedidoItemRequest
            {
                NomeProduto = row["NomeProduto"],
                Quantidade = int.Parse(row["Quantidade"]),
                Valor = decimal.Parse(row["Valor"])
            });
        }

        _pedidoRequest = new PedidoCreateRequest
        {
            Cliente = "Cliente Teste",
            ClienteId = 1,
            Valor = 130.00m,
            Itens = itens
        };
    }

    [When(@"eu enviar a solicitação de criação do pedido")]
    public async Task QuandoEuEnviarASolicitacaoDeCriacaoDoPedido()
    {
        var pedidoCriado = new Pedido
        {
            Id = 1,
            Cliente = _pedidoRequest.Cliente,
            ClienteId = _pedidoRequest.ClienteId,
            Valor = _pedidoRequest.Valor,
            Status = PedidoStatus.Criado,
            Itens = new List<PedidoItem>()
        };

        _pedidoRepositoryMock.Setup(r => r.AdicionarPedido(It.IsAny<Pedido>())).ReturnsAsync(pedidoCriado);
        _pedidoResponse = await _pedidoUseCase.CriarPedido(_pedidoRequest);
    }

    [Then(@"o pedido deve ser criado com sucesso")]
    public void EntaoOPedidoDeveSerCriadoComSucesso()
    {
        Assert.NotNull(_pedidoResponse);
        Assert.Equal(_pedidoRequest.Cliente, _pedidoResponse.Cliente);
    }

    [Then(@"o status do pedido deve ser ""(.*)""")]
    public void EntaoOStatusDoPedidoDeveSer(string status)
    {
        Assert.Equal(status, _pedidoResponse.Status);
    }
}