﻿using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataLayer.Models.DB
{
    public partial class FacturadorWebContext : DbContext
    {
        public FacturadorWebContext()
        {
        }

        public FacturadorWebContext(DbContextOptions<FacturadorWebContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<EstadosFactura> EstadosFactura { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<Inventario> Inventario { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-HTNHL6CO\\SQLEXPRESS;Database=FacturadorWeb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("CLIENTE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdPersona).HasColumnName("ID_PERSONA");

                entity.Property(e => e.Puntos).HasColumnName("PUNTOS");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("FK__CLIENTE__ID_PERS__3F466844");
            });

            modelBuilder.Entity<EstadosFactura>(entity =>
            {
                entity.ToTable("ESTADOS_FACTURA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable("FACTURA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FechaApertura)
                    .HasColumnName("FECHA_APERTURA")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaCierre)
                    .HasColumnName("FECHA_CIERRE")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdEstado).HasColumnName("ID_ESTADO");

                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");

                entity.Property(e => e.Numero)
                    .HasColumnName("NUMERO")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ValorDescuento)
                    .HasColumnName("VALOR_DESCUENTO")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ValorIva)
                    .HasColumnName("VALOR_IVA")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ValorSubtotal)
                    .HasColumnName("VALOR_SUBTOTAL")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ValorTotal)
                    .HasColumnName("VALOR_TOTAL")
                    .HasColumnType("decimal(18, 3)");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Factura)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK__FACTURA__ID_CLIE__1DB06A4F");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Factura)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("FK__FACTURA__ID_ESTA__1BC821DD");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Factura)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__FACTURA__ID_USUA__1CBC4616");
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.ToTable("INVENTARIO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnName("FECHA_REGISTRO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.PorcentajeDescuento)
                    .HasColumnName("PORCENTAJE_DESCUENTO")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PorcentajeIva)
                    .HasColumnName("PORCENTAJE_IVA")
                    .HasColumnType("decimal(18, 1)");

                entity.Property(e => e.PrecioCompra)
                    .HasColumnName("PRECIO_COMPRA")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.PrecioVenta)
                    .HasColumnName("PRECIO_VENTA")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TotalDesincorporados)
                    .HasColumnName("TOTAL_DESINCORPORADOS")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TotalDevueltos)
                    .HasColumnName("TOTAL_DEVUELTOS")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TotalProceso)
                    .HasColumnName("TOTAL_PROCESO")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TotalRecibidos)
                    .HasColumnName("TOTAL_RECIBIDOS")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TotalVendidos)
                    .HasColumnName("TOTAL_VENDIDOS")
                    .HasColumnType("decimal(18, 3)");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("PERSONA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasColumnName("APELLIDO")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasColumnName("DIRECCION")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipoDocumento).HasColumnName("ID_TIPO_DOCUMENTO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDocumento)
                    .IsRequired()
                    .HasColumnName("NUMERO_DOCUMENTO")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasColumnName("TELEFONO")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTipoDocumentoNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdTipoDocumento)
                    .HasConstraintName("FK__PERSONA__ID_TIPO__398D8EEE");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("PRODUCTO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cantidad)
                    .HasColumnName("CANTIDAD")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.IdFactura).HasColumnName("ID_FACTURA");

                entity.Property(e => e.IdInventario).HasColumnName("ID_INVENTARIO");

                entity.Property(e => e.PorcentajeDescuento)
                    .HasColumnName("PORCENTAJE_DESCUENTO")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.PorcentajeIva)
                    .HasColumnName("PORCENTAJE_IVA")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ValorTotal)
                    .HasColumnName("VALOR_TOTAL")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ValorTotalDescuento)
                    .HasColumnName("VALOR_TOTAL_DESCUENTO")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ValorTotalIva)
                    .HasColumnName("VALOR_TOTAL_IVA")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ValorUnitario)
                    .HasColumnName("VALOR_UNITARIO")
                    .HasColumnType("decimal(18, 3)");

                entity.HasOne(d => d.IdFacturaNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.IdFactura)
                    .HasConstraintName("FK__PRODUCTO__ID_FAC__2180FB33");

                entity.HasOne(d => d.IdInventarioNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.IdInventario)
                    .HasConstraintName("FK__PRODUCTO__ID_INV__208CD6FA");
            });

            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.ToTable("TIPO_DOCUMENTO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Acronimo)
                    .IsRequired()
                    .HasColumnName("ACRONIMO")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasColumnName("CLAVE")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasColumnName("ESTADO");

                entity.Property(e => e.IdPersona).HasColumnName("ID_PERSONA");

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasColumnName("NICKNAME")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Perfil)
                    .IsRequired()
                    .HasColumnName("PERFIL")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("FK__USUARIO__ID_PERS__3C69FB99");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
