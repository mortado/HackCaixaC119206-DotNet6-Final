using HackCaixa.Application.Repositories;
using HackCaixa.Application.Repositories.Interfaces;
using HackCaixa.Application.Services;
using Moq;
using Xunit;

namespace HackCaixa.Tests.Services
{
    public class ProdutoServiceTests
    {
        private ProdutoService _produtoServiceMock;        
        public ProdutoServiceTests() 
        {         
            //Aqui utilizei o Mock para injetar.            
            _produtoServiceMock = new ProdutoService(new Mock<IProdutoRepository>().Object);            
        }

        [Fact]
        public void ListarProduto()
        {
            var result = _produtoServiceMock.ListarProdutos();
            Assert.NotNull(result);
        }

        [Fact]
        public void ListarMelhorProduto()
        {            
            var result = _produtoServiceMock.ListarMelhorProduto(10,10);
            Assert.NotNull(result);
        }

        [Fact]
        public void ListarProdutoProximo()
        {
            var result = _produtoServiceMock.ListarProdutoProximo(1000, 10);
            Assert.NotNull(result);
        }
    }
}
