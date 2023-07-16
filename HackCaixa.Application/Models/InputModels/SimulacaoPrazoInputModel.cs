
using HackCaixa.Application.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.InputModels
{
    public class SimulacaoPrazoInputModel
    {
        [Required(ErrorMessage = "O campo 'prazo' é obrigatório.")]
        public short Prazo { get; set; }

        public PessoaViewModel? Contato { get; set; }

    }
}
