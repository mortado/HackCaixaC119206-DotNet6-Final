
using HackCaixa.Application.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.InputModels
{
    public class SimulacaoValorInputModel
    {
        [Required(ErrorMessage = "O campo 'valorDesejado' é obrigatório.")]
        public decimal ValorDesejado { get; set; }

        public PessoaViewModel? Contato { get; set; }

    }
}
