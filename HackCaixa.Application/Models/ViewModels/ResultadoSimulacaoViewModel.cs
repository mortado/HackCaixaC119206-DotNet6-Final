using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.ViewModels
{
    public class ResultadoSimulacaoViewModel
    {
        public string? Tipo { get; set; }
        public List<ParcelaViewModel>? Parcelas { get; set; }
    }
}
