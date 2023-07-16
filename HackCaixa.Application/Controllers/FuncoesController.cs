using HackCaixa.Application.Models;
using HackCaixa.Application.Models.InputModels;
using HackCaixa.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace HackCaixa.Application.Controllers
{
    public class FuncoesController : ControllerBase
    {

        [Description("Essa função retorna o Resultado da Simulação para os sistemas PRICE e SAC." +
            "Não usa o arredondamento das casas decimais no início dos cálculos. " +
            "Ela calcula exatamente como a Calculadora SAC e Price do CALCULOJURIDICO")]
        public SimulacaoViewModel Simular(ProdutoViewModel produto, SimulacaoInputModel solicitacao)
        {
            return Simular(false, produto, solicitacao);
        }


        [Description("Essa função retorna o Resultado da Simulação para os sistemas PRICE e SAC. " +
            "Pode definir se usa arredondamento das casas decimais desde o início do calculo ou se usa o arredondamento somente no final, após gerar todo o cálculo. " +
            "Para retornar os dados EXATAMENTE como a API CAIXA DO HACKATON, usa o arredondamento como TRUE" +
            "Para retornar os dados EXATAMENTE como a Calculadora SAC e Price do CALCULOJURIDICO, usa o arredondamento como  FALSE")]
        public SimulacaoViewModel Simular(bool usarArredondamentoDesdeOInicio, ProdutoViewModel produto, SimulacaoInputModel solicitacao)
        {            
            return new SimulacaoViewModel
            {
                CodigoProduto = produto.CoProduto,
                DescricaoProduto = produto.NoProduto,
                TaxaJuros = produto.PcTaxaJuros,
                ResultadoSimulacao = CalcularSACPRICE(usarArredondamentoDesdeOInicio, solicitacao.ValorDesejado, solicitacao.Prazo, produto.PcTaxaJuros ?? 0)
            };
        }
        

        [Description("Essa função segrega em um objeto o resultado com a simulação PRICE e SAC." +
            "Na API CAIXA observei que foi utilizado o arredondamento antes de cada cálculo. Neste caso, se quiser utilizar dessa forma, basta informar o" +
            "parâmetro 'usarArredondamentoDesdeOInicio' como true.")]
        private List<ResultadoSimulacaoViewModel> CalcularSACPRICE(bool usarArredondamentoDesdeOInicio, decimal valorDesejado, int prazo, decimal taxa)
        {
            List<ResultadoSimulacaoViewModel> resultado = new List<ResultadoSimulacaoViewModel>();

            // Simulação para SAC
            resultado.Add(new ResultadoSimulacaoViewModel
            {
                Tipo = "SAC",
                Parcelas = CalculoSAC(usarArredondamentoDesdeOInicio, valorDesejado, prazo, taxa)
            });

            // Simulação para PRICE
            resultado.Add(new ResultadoSimulacaoViewModel
            {
                Tipo = "PRICE",
                Parcelas = CalculoPRICE(usarArredondamentoDesdeOInicio, valorDesejado, prazo, taxa)
            });

            return resultado;

        }


        [Description("Essa função calcula as parcelas utilizando o Sistema PRICE")]
        private List<ParcelaViewModel> CalculoPRICE(bool usarArredondamentoDesdeOInicio, decimal valorDesejado, int prazo, decimal taxa)
        {
            // Calculo da Prestação pela formula PRICE
            decimal prestacao = (valorDesejado * taxa) / (1 - (decimal)Math.Pow((double)(1 + taxa), -prazo));
            if (usarArredondamentoDesdeOInicio)
                prestacao = Math.Round(prestacao, 2);

            decimal saldoDevedor = valorDesejado;
            List<ParcelaViewModel> parcelas = new List<ParcelaViewModel>();

            for (int i = 0; i < prazo; i++)
            {
                // Calculo do juros
                decimal juros = saldoDevedor * taxa;
                if (usarArredondamentoDesdeOInicio)
                    juros = Math.Round(juros, 2);


                // Calculo da Amortização
                decimal amortizacao = prestacao - juros;
                if (usarArredondamentoDesdeOInicio)
                    amortizacao = Math.Round(amortizacao, 2);


                // Geração do Objeto
                parcelas.Add(new ParcelaViewModel
                {
                    Numero = i + 1,
                    ValorAmortizacao = Math.Round(amortizacao,2),
                    ValorJuros = Math.Round(juros,2),
                    ValorPrestacao = Math.Round(prestacao,2)
                });


                // Redução do Saldo Devedor
                saldoDevedor -= amortizacao;
            }

            return parcelas;
        }

        [Description("Essa função calcula as parcelas utilizando o Sistema SAC")]
        private List<ParcelaViewModel> CalculoSAC(bool usarArredondamentoDesdeOInicio, decimal valorDesejado, int prazo, decimal taxa)
        {
            // Calculo da Amortização pela formula SAC
            decimal saldoDevedor = valorDesejado;
            decimal amortizacao = saldoDevedor / prazo;
            if (usarArredondamentoDesdeOInicio)
                amortizacao = Math.Round(amortizacao, 2);

            List<ParcelaViewModel> parcelas = new List<ParcelaViewModel>();

            for (int i = 0; i < prazo; i++)
            {
                // Calculo do Juros
                decimal juros = saldoDevedor * taxa;
                if (usarArredondamentoDesdeOInicio)
                    juros = Math.Round(juros, 2);


                // Calculo da Prestação
                decimal prestacao = juros + amortizacao;


                // Geração do Objeto
                parcelas.Add(new ParcelaViewModel
                {
                    Numero = i + 1,
                    ValorAmortizacao = Math.Round(amortizacao, 2),
                    ValorJuros = Math.Round(juros, 2),
                    ValorPrestacao = Math.Round(prestacao, 2)
                });

                // Redução do Saldo Devedor
                saldoDevedor -= amortizacao;
            }

            return parcelas;
        }        

    }
}
