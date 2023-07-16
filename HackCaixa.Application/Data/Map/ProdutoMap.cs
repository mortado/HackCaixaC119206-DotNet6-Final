using HackCaixa.Application.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackCaixa.Application.Data.Map
{
    public class ProdutoMap : IEntityTypeConfiguration<ProdutoViewModel>
    {
        public void Configure(EntityTypeBuilder<ProdutoViewModel> builder)
        {
            builder.HasKey(x => x.CoProduto);
        }
    }
}
