using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackCaixa.Application.Models.ViewModels
{
    public class ProdutoViewModel
    {
        [Key]
        [Column("co_produto")]
        public int CoProduto { get; set; }

        [Column("no_produto")]
        public string? NoProduto { get; set; }

        [Column("pc_taxa_juros")]
        public decimal? PcTaxaJuros { get; set; }

        [Column("nu_minimo_meses")]
        public short? NuMinimoMeses { get; set; }

        [Column("nu_maximo_meses")]
        public short? NuMaximoMeses { get; set; }

        [Column("vr_minimo")]
        public decimal? VrMinimo { get; set; }

        [Column("vr_maximo")]
        public decimal? VrMaximo { get; set; }

    }
}
