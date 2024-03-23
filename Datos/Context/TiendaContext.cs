using System;
using System.Collections.Generic;
using Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Datos.Context
{
    public partial class TiendaContext : DbContext
    {
        public TiendaContext()
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public TiendaContext(DbContextOptions<TiendaContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Articulo> Articulos { get; set; } = null!;
        public virtual DbSet<ArticuloTienda> ArticuloTienda { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<ClienteArticulo> ClienteArticulos { get; set; } = null!;
        public virtual DbSet<Tienda> Tiendas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
                var connectionString = configuration.GetConnectionString("connApp");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Imagen)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ArticuloTienda>(entity =>
            {
                entity.HasKey(sc => new { sc.ArticuloId, sc.TiendaId });

                entity.Property(e => e.ArticuloId).HasColumnName("Articulo_ID");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.TiendaId).HasColumnName("Tienda_ID");

                entity.HasOne(d => d.Articulo)
                    .WithMany(s => s.ArticuloTiendas)
                    .HasForeignKey(d => d.ArticuloId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Articulo_ID");

                entity.HasOne(d => d.Tienda)
                    .WithMany(s => s.ArticuloTiendas)
                    .HasForeignKey(d => d.TiendaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tienda_ID");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Contrasena)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasIndex(e => e.Correo).IsUnique();
            });

            modelBuilder.Entity<ClienteArticulo>(entity =>
            {
                entity.HasKey(sc => new { sc.ClienteId, sc.ArticuloId });

                entity.ToTable("ClienteArticulo");

                entity.Property(e => e.ArticuloId).HasColumnName("Articulo_ID");

                entity.Property(e => e.ClienteId).HasColumnName("Cliente_ID");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.HasOne(d => d.Articulo)
                    .WithMany(s => s.ClienteArticulos)
                    .HasForeignKey(d => d.ArticuloId)
                    .HasConstraintName("FK_Articulos_ID");

                entity.HasOne(d => d.Cliente)
                    .WithMany(s => s.ClienteArticulos)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK_Cliente_ID");
            });

            modelBuilder.Entity<Tienda>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Direccion).HasMaxLength(200);

                entity.Property(e => e.Sucursal)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
