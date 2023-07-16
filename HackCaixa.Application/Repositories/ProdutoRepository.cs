using HackCaixa.Application.Repositories.Interfaces;
using HackCaixa.Application.Data;
using HackCaixa.Application.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HackCaixa.Application.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ProdutoDBContext _dbcontext;
        public ProdutoRepository(ProdutoDBContext produtoDbContext)
        {
            _dbcontext = produtoDbContext;
        }

        [Description("Essa função retorna todos or produtos disponíveis.")]
        public async Task<List<ProdutoViewModel>> ListarProdutos()
        {
            return await _dbcontext.produto.ToListAsync();
        }

        [Description("Essa função retorna os produtos disponíveis para o filtro selecionado")]
        public async Task<List<ProdutoViewModel>> ListarMelhorProduto(decimal valorDesejavel, short prazo)
        {
            return await _dbcontext.produto.Where(x => x.NuMinimoMeses <= prazo)
                .Where(x => x.NuMaximoMeses >= prazo || x.NuMaximoMeses == null)
                .Where(x => x.VrMinimo <= valorDesejavel)
                .Where(x => x.VrMaximo >= valorDesejavel || x.VrMaximo == null)

                .ToListAsync();
        }


        [Description("Essa função retorna os produtos mais próximos para o filtro selecionado")]
        public async Task<ProdutoProximoViewModel> ListarProdutoProximo(decimal valorDesejavel, short prazo)
        {
            ProdutoViewModel porValor = await _dbcontext.produto
                .Where(x => x.VrMinimo <= valorDesejavel)
                .Where(x => x.VrMaximo >= valorDesejavel || x.VrMaximo == null)
                .FirstOrDefaultAsync() ?? new ProdutoViewModel();

            ProdutoViewModel porPrazo = await _dbcontext.produto
                .Where(x => x.NuMinimoMeses <= prazo)
                .Where(x => x.NuMaximoMeses >= prazo).FirstOrDefaultAsync() ?? new ProdutoViewModel();

            return new ProdutoProximoViewModel { PorValor = porValor, PorPrazo = porPrazo };

        }
    }
}
