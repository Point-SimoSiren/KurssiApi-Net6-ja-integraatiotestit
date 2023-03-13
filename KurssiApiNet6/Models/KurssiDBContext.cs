using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KurssiApiNet6.Models
{
    public partial class KurssiDBContext : DbContext
    {
        public KurssiDBContext()
        {
        }

        public KurssiDBContext(DbContextOptions<KurssiDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Kurssit> Kurssit { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-05S0KN9\\SQLEXPRESS;Database=KurssiDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kurssit>(entity =>
            {
                entity.HasKey(e => e.KurssiId);

                entity.ToTable("Kurssit");

                entity.Property(e => e.KurssiId).HasColumnName("KurssiID");

                entity.Property(e => e.Nimi).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
