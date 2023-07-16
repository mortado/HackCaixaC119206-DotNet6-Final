using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.ViewModels
{
    public class SimulacaoCustomViewModel
    {
        public decimal Valor  { get; set; }

        public short Prazo { get; set; }
        
        public SimulacaoViewModel Simulacao { get; set; }
    }
}
