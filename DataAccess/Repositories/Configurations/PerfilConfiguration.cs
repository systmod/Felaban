﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace DataAccess.Repositories.Configurations
{
    public partial class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> entity)
        {
            entity.HasKey(e => e.IdPerfil);

            entity.Property(e => e.IdPerfil).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Codigo)
            .IsRequired()
            .HasMaxLength(10)
            .IsUnicode(false);
            entity.Property(e => e.Descripcion)
            .IsRequired()
            .HasMaxLength(250)
            .IsUnicode(false);
            entity.Property(e => e.FechaIngreso).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.IdEstado).HasDefaultValueSql("((1))");
            entity.Property(e => e.IdProducto).HasDefaultValueSql("((1))");
            entity.Property(e => e.IdTipoPerfil).HasDefaultValueSql("((2))");
            entity.Property(e => e.IpIngreso)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);
            entity.Property(e => e.IpModificacion)
            .HasMaxLength(15)
            .IsUnicode(false);
            entity.Property(e => e.UsuarioIngreso)
            .HasMaxLength(50)
            .IsUnicode(false);
            entity.Property(e => e.UsuarioModificacion)
            .HasMaxLength(50)
            .IsUnicode(false);

            entity.HasOne(d => d.IdAplicacionNavigation).WithMany(p => p.Perfil)
            .HasForeignKey(d => d.IdAplicacion)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Perfil_Aplicacion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Perfil)
            .HasForeignKey(d => d.IdProducto)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Perfil_Producto");

            entity.HasOne(d => d.IdTipoPerfilNavigation).WithMany(p => p.Perfil)
            .HasForeignKey(d => d.IdTipoPerfil)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Perfil_TipoPerfil");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Perfil> entity);
    }
}
