using HackCaixa.Application.Models.ViewModels;

namespace HackCaixa.Application.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<ProdutoViewModel>> ListarProdutos();
        Task<List<ProdutoViewModel>> ListarMelhorProduto(decimal valorDesejavel, short prazo);
        Task<ProdutoProximoViewModel> ListarProdutoProximo(decimal valorDesejavel, short prazo);
    }
}
