﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace DataAccess.Repositories.Configurations
{
    public partial class UsuarioUnidadConfiguration : IEntityTypeConfiguration<UsuarioUnidad>
    {
        public void Configure(EntityTypeBuilder<UsuarioUnidad> entity)
        {
            entity.HasKey(e => e.IdUsuarioUnidad).HasName("PK_Acceso");

            entity.HasIndex(e => new { e.IdUsuario, e.IdEmpresa, e.IdAplicacion, e.IdUnidadAdmin }, "IX_UsuarioUnidad").IsUnique();

            entity.Property(e => e.IdUsuario)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

            entity.HasOne(d => d.IdAplicacionNavigation).WithMany(p => p.UsuarioUnidad)
            .HasForeignKey(d => d.IdAplicacion)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UsuarioUnidad_Aplicacion");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.UsuarioUnidad)
            .HasForeignKey(d => d.IdEmpresa)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UsuarioUnidad_Empresa");

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.UsuarioUnidad)
            .HasForeignKey(d => d.IdPerfil)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UsuarioUnidad_Perfil");

            entity.HasOne(d => d.IdUnidadAdminNavigation).WithMany(p => p.UsuarioUnidad)
            .HasForeignKey(d => d.IdUnidadAdmin)
            .HasConstraintName("FK_UsuarioUnidad_UnidadAdmin");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioUnidad)
            .HasForeignKey(d => d.IdUsuario)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UsuarioEmpresa_Usuario");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UsuarioUnidad> entity);
    }
}
