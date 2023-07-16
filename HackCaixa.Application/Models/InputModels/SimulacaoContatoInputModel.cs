
using HackCaixa.Application.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.InputModels
{
    public class SimulacaoContatoInputModel
    {
        [Required(ErrorMessage = "O campo 'simulacao' é obrigatório.")]
        public SimulacaoInputModel Simulacao { get; set; }

        [Required(ErrorMessage = "O campo 'contato' é obrigatório.")]
        public PessoaViewModel Contato { get; set; }

    }
}
