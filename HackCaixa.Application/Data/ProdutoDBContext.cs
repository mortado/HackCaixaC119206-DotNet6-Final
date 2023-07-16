using HackCaixa.Application.Models.ViewModels;
using HackCaixa.Application.Repositories;
using HackCaixa.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HackCaixa.Application.Data
{
    public class ProdutoDBContext : DbContext
    {

        public ProdutoDBContext(DbContextOptions<ProdutoDBContext> options) : base(options) {}

        public DbSet<ProdutoViewModel> produto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);
        }
    }
}
