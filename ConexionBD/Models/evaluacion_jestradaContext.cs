using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;



namespace ConexionBD.Models
{
    public partial class evaluacion_jestradaContext : DbContext
    {
        public evaluacion_jestradaContext()
        {
        }

        public evaluacion_jestradaContext(DbContextOptions<evaluacion_jestradaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Resuman> Resumen { get; set; } = null!;
        public virtual DbSet<Tiket> Tikets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();


                string connectionString = configuration.GetConnectionString("GA");


                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resuman>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.IdRegistradora)
                    .HasMaxLength(50)
                    .HasColumnName("Id_Registradora");

                entity.Property(e => e.IdTienda)
                    .HasMaxLength(50)
                    .HasColumnName("Id_Tienda");
            });

            modelBuilder.Entity<Tiket>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.FechaHoraCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FechaHora_Creacion");

                entity.Property(e => e.IdRegistradora)
                    .HasMaxLength(50)
                    .HasColumnName("Id_Registradora");

                entity.Property(e => e.IdTienda)
                    .HasMaxLength(50)
                    .HasColumnName("Id_Tienda");

                entity.Property(e => e.Impuesto).HasColumnType("money");

                entity.Property(e => e.Total).HasColumnType("money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
