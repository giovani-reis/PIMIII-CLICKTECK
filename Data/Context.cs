using Microsoft.EntityFrameworkCore;
using PIM_TechTrust.Models;

namespace PIMIII_CLICKTECK.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TecnicoPerfil> TecnicoPerfis { get; set; }
        public DbSet<Especialidade> Especialidades { get; set; }
        public DbSet<TecnicoEspecialidade> TecnicoEspecialidades { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 1:1 Usuario -> TecnicoPerfil
            builder.Entity<TecnicoPerfil>()
                .HasOne(t => t.Usuario)
                .WithOne(u => u.TecnicoPerfil)
                .HasForeignKey<TecnicoPerfil>(t => t.UsuarioId);

            // N:N TecnicoEspecialidade
            builder.Entity<TecnicoEspecialidade>()
                .HasKey(te => new { te.TecnicoPerfilId, te.EspecialidadeId });

            // N:N Favoritos
            builder.Entity<Favorito>()
                .HasKey(f => new { f.ClienteId, f.TecnicoId });

            // Configurações de FK Duplas (Blindagem contra Shadow Properties)
            builder.Entity<Atendimento>(e => {
                e.HasOne(a => a.Cliente).WithMany().HasForeignKey(a => a.ClienteId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(a => a.Tecnico).WithMany().HasForeignKey(a => a.TecnicoId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Avaliacao>(e => {
                e.HasOne(a => a.Cliente).WithMany().HasForeignKey(a => a.ClienteId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(a => a.Tecnico).WithMany().HasForeignKey(a => a.TecnicoId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Mensagem>(e => {
                e.HasOne(m => m.Remetente).WithMany().HasForeignKey(m => m.RemetenteId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(m => m.Destinatario).WithMany().HasForeignKey(m => m.DestinatarioId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Favorito>(e => {
                e.HasOne(f => f.Cliente).WithMany().HasForeignKey(f => f.ClienteId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(f => f.Tecnico).WithMany().HasForeignKey(f => f.TecnicoId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
