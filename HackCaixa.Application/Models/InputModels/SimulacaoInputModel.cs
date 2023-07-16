
using HackCaixa.Application.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.InputModels
{
    public class SimulacaoInputModel
    {
        [Required(ErrorMessage = "O campo 'valorDesejado' é obrigatório.")]
        public decimal ValorDesejado { get; set; }

        [Required(ErrorMessage = "O campo 'prazo' é obrigatório.")]
        public short Prazo { get; set; }

    }
}
