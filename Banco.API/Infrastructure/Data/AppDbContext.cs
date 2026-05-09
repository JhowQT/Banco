using Microsoft.EntityFrameworkCore;
using Banco.API.Domain.Entities;

namespace Banco.API.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }

        public DbSet<Agencia> Agencias { get; set; }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        public DbSet<Contratacao> Contratacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // HERANÇA CLIENTE
            // =========================
            modelBuilder.Entity<Cliente>()
                .HasDiscriminator<string>("TipoCliente")
                .HasValue<PessoaFisica>("PF")
                .HasValue<PessoaJuridica>("PJ");

            // =========================
            // HERANÇA PRODUTO
            // =========================
            modelBuilder.Entity<Produto>()
                .HasDiscriminator<string>("TipoProduto")
                .HasValue<Emprestimo>("EMPRESTIMO");

            // =========================
            // AGENCIA -> CLIENTES
            // =========================
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Agencia)
                .WithMany(a => a.Clientes)
                .HasForeignKey(c => c.AgenciaId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // CLIENTE -> CONTRATACOES
            // =========================
            modelBuilder.Entity<Contratacao>()
                .HasOne(c => c.Cliente)
                .WithMany(c => c.Contratacoes)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // PRODUTO -> CONTRATACOES
            // =========================
            modelBuilder.Entity<Contratacao>()
                .HasOne(c => c.Produto)
                .WithMany()
                .HasForeignKey(c => c.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // CAMPOS OBRIGATÓRIOS
            // =========================
            modelBuilder.Entity<Agencia>()
                .Property(a => a.Nome)
                .IsRequired();

            modelBuilder.Entity<PessoaFisica>()
                .Property(p => p.CPF)
                .IsRequired();

            modelBuilder.Entity<PessoaJuridica>()
                .Property(p => p.CNPJ)
                .IsRequired();

            modelBuilder.Entity<Contratacao>()
                .Property(c => c.Status)
                .IsRequired();

            // =========================
            // DECIMAL ORACLE
            // =========================
            modelBuilder.Entity<Emprestimo>()
                .Property(e => e.Valor)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Emprestimo>()
                .Property(e => e.TaxaJuros)
                .HasPrecision(5, 2);
        }
    }
}