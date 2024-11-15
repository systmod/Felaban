﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace DataAccess.Repositories.Configurations
{
    public partial class ProductoEmpresaConfiguration : IEntityTypeConfiguration<ProductoEmpresa>
    {
        public void Configure(EntityTypeBuilder<ProductoEmpresa> entity)
        {
            entity.HasKey(e => e.IdProductoEmpresa).HasName("PK_AplicacionEmpresa");

            entity.Property(e => e.FechaActivacion)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("date");
            entity.Property(e => e.FechaExpiracion).HasColumnType("date");
            entity.Property(e => e.IdLicencia).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ProductoEmpresa)
            .HasForeignKey(d => d.IdEmpresa)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ProductoEmpresa_Empresa");

            entity.HasOne(d => d.IdLicenciaNavigation).WithMany(p => p.ProductoEmpresa)
            .HasForeignKey(d => d.IdLicencia)
            .HasConstraintName("FK_ProductoEmpresa_Licencia");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductoEmpresa)
            .HasForeignKey(d => d.IdProducto)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ProductoEmpresa_Producto");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ProductoEmpresa> entity);
    }
}
