using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.ViewModels
{
    public class SimulacaoSemResultadoHubViewModel
    {
        public decimal ValorDesejado { get; set; }
        public short Prazo { get; set; }
        public string Mensagem { get; set; } = "";
        public ProdutoProximoViewModel? Sugestao { get; set; }

        public PessoaViewModel? Contato { get; set; }
    }
}
