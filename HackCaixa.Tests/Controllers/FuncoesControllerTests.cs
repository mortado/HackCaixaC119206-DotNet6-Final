using HackCaixa.Application.Controllers;
using HackCaixa.Application.Models.InputModels;
using HackCaixa.Application.Models.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HackCaixa.Tests.Controllers
{
    public class FuncoesControllerTests
    {
        private readonly FuncoesController _funcoes;
        public FuncoesControllerTests() 
        { 
            _funcoes = new FuncoesController();
        }


        [Fact]
        public void Calcular_UsingArredondamentoTrue()
        {
            ProdutoViewModel produto = new ProdutoViewModel {
                CoProduto = 1,
                NoProduto = "Teste 1",
                PcTaxaJuros = (decimal)0.0179,
                NuMaximoMeses = 24,
                NuMinimoMeses = 1,
                VrMaximo = 20000,
                VrMinimo = 1000
            };

            SimulacaoInputModel solicitacao = new SimulacaoInputModel
            {
                ValorDesejado = 1000,
                Prazo = 5
            };

            var result = _funcoes.Simular(true, produto, solicitacao);
            var validacao = true;
            if (result.ResultadoSimulacao.IsNullOrEmpty())
                Assert.Fail("Não veio a simulação");
            else
            {
                //SAC
                validacao = ((result.ResultadoSimulacao?[0].Parcelas?[3].ValorPrestacao == (decimal)207.16) && validacao);
                validacao = ((result.ResultadoSimulacao?[0].Parcelas?[2].ValorPrestacao == (decimal)210.74) && validacao);
                validacao = ((result.ResultadoSimulacao?[0].Parcelas?[1].ValorJuros == (decimal)14.32) && validacao);
                validacao = ((result.ResultadoSimulacao?[0].Parcelas?[0].ValorAmortizacao == (decimal)200) && validacao);

                //PRICE
                validacao = ((result.ResultadoSimulacao?[1].Parcelas?[3].ValorPrestacao == (decimal)210.87) && validacao);
                validacao = ((result.ResultadoSimulacao?[1].Parcelas?[2].ValorAmortizacao == (decimal)199.94) && validacao);
                validacao = ((result.ResultadoSimulacao?[1].Parcelas?[1].ValorJuros == (decimal)14.45) && validacao);
                validacao = ((result.ResultadoSimulacao?[1].Parcelas?[0].ValorAmortizacao == (decimal)192.97) && validacao);
                Assert.True(validacao);
            }            
        }

        [Fact]
        public void Calcular_UsingArredondamentoFalse()
        {
            ProdutoViewModel produto = new ProdutoViewModel
            {
                CoProduto = 1,
                NoProduto = "Teste 1",
                PcTaxaJuros = (decimal)0.0179,
                NuMaximoMeses = 24,
                NuMinimoMeses = 1,
                VrMaximo = 20000,
                VrMinimo = 1000
            };

            SimulacaoInputModel solicitacao = new SimulacaoInputModel
            {
                ValorDesejado = 10000,
                Prazo = 20
            };

            var result = _funcoes.Simular(false, produto, solicitacao);
            var validacao = true;
            if (result.ResultadoSimulacao.IsNullOrEmpty())
                Assert.Fail("Não veio a simulação");
            else
            {
                //SAC
                validacao = ((result.ResultadoSimulacao?[0].Parcelas?[19].ValorPrestacao == (decimal)508.95) && validacao);
                validacao = ((result.ResultadoSimulacao?[0].Parcelas?[18].ValorPrestacao == (decimal)517.9) && validacao);
                validacao = ((result.ResultadoSimulacao?[0].Parcelas?[10].ValorJuros == (decimal)89.5) && validacao);
                validacao = ((result.ResultadoSimulacao?[0].Parcelas?[1].ValorAmortizacao == (decimal)500) && validacao);

                //PRICE
                validacao = ((result.ResultadoSimulacao?[1].Parcelas?[19].ValorPrestacao == (decimal)599.24) && validacao);
                validacao = ((result.ResultadoSimulacao?[1].Parcelas?[18].ValorAmortizacao == (decimal)578.35) && validacao);
                validacao = ((result.ResultadoSimulacao?[1].Parcelas?[10].ValorJuros == (decimal)97.42) && validacao);
                validacao = ((result.ResultadoSimulacao?[1].Parcelas?[0].ValorAmortizacao == (decimal)420.24) && validacao);
                Assert.True(validacao);
            }
        }

    }
}
