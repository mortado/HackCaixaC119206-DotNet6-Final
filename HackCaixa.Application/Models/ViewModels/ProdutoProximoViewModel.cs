using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HackCaixa.Application.Models.ViewModels
{
    public class ProdutoProximoViewModel
    {
        public ProdutoViewModel? PorValor { get; set; }
        public ProdutoViewModel? PorPrazo { get; set; }

    }
}
