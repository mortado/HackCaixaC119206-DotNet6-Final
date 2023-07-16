using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.ViewModels
{
    public class SimulacaoViewModel
    {

        public int CodigoProduto { get; set; }
        public string? DescricaoProduto { get; set; }
        public decimal? TaxaJuros { get; set; }
        public List<ResultadoSimulacaoViewModel>? ResultadoSimulacao { get; set; }
    }
}
