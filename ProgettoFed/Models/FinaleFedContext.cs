using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProgettoFed.Models
{
    public partial class FinaleFedContext : DbContext
    {
        public FinaleFedContext()
        {
        }

        public FinaleFedContext(DbContextOptions<FinaleFedContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Notum> Nota { get; set; } = null!;
        public virtual DbSet<Utente> Utentes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=FinaleFed;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notum>(entity =>
            {
                entity.HasKey(e => e.IdTask);

                entity.Property(e => e.IdTask).HasColumnName("ID_Task");

                entity.Property(e => e.DataCreazione).HasColumnType("datetime");

                entity.Property(e => e.DataScadenza).HasColumnType("datetime");

                entity.Property(e => e.IdUtente).HasColumnName("ID_Utente");

                entity.Property(e => e.TestoTask)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TitoloTask)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUtenteNavigation)
                    .WithMany(p => p.Nota)
                    .HasForeignKey(d => d.IdUtente)
                    .HasConstraintName("FK_Nota_Utente");
            });

            modelBuilder.Entity<Utente>(entity =>
            {
                entity.HasKey(e => e.IdUtente);

                entity.ToTable("Utente");

                entity.Property(e => e.IdUtente).HasColumnName("ID_Utente");

                entity.Property(e => e.Nome)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Psw)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
