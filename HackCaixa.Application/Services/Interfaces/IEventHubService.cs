using HackCaixa.Application.Models;
using HackCaixa.Application.Models.ViewModels;

namespace HackCaixa.Application.Services.Interfaces
{
    public interface IEventHubService
    {
        Task EnviarEventoParaEventHub(SimulacaoHubViewModel resultado)
        {
            throw new NotImplementedException();
        }

        Task EnviarEventoParaEventHubSemProduto(SimulacaoSemResultadoHubViewModel resultado)
        {
            throw new NotImplementedException();
        }
    }
}
