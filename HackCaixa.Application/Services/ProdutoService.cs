using HackCaixa.Application.Models.ViewModels;
using HackCaixa.Application.Repositories.Interfaces;
using HackCaixa.Application.Services.Interfaces;


namespace HackCaixa.Application.Services
{
    public class ProdutoService : IProdutoService
    {

        private readonly IProdutoRepository _repositorio;

        public ProdutoService(IProdutoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<ProdutoViewModel>> ListarProdutos()
        {
            return await _repositorio.ListarProdutos();
        }

        public async Task<List<ProdutoViewModel>> ListarMelhorProduto(decimal valorDesejavel, short prazo)
        {
            return await _repositorio.ListarMelhorProduto(valorDesejavel, prazo);
        }

        public async Task<ProdutoProximoViewModel> ListarProdutoProximo(decimal valorDesejavel, short prazo)
        {
            return await _repositorio.ListarProdutoProximo(valorDesejavel, prazo);
        }


    }
}
