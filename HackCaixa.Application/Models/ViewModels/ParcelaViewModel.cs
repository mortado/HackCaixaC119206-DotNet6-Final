using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.ViewModels
{
    public class ParcelaViewModel
    {
        public int Numero { get; set; }
        public decimal? ValorAmortizacao { get; set; }
        public decimal? ValorJuros { get; set; }
        public decimal? ValorPrestacao { get; set; }

    }
}
