using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.ViewModels
{
    public class ErroProximoViewModel
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; } = "";
        public ProdutoProximoViewModel? Sugestoes { get; set; }

    }
}
