using Microsoft.EntityFrameworkCore;
using SiteMVC.Models;

namespace SiteMVC.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        public DbSet<ContatoModel> Contatos { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContatoModel>().ToTable("Contatos");
            modelBuilder.Entity<UsuarioModel>().ToTable("Usuarios");
            // Adicione outras configurações aqui se necessário
        }
    }
}