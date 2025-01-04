﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ConcentratorFraud.DataAccess.Models;
using ConcentratorFraud.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ConcentratorFraud.DataAccess.Repositories.Configurations
{
    public partial class IncidenteConfiguration : IEntityTypeConfiguration<Incidente>
    {
        public void Configure(EntityTypeBuilder<Incidente> entity)
        {
            entity.HasKey(e => e.IdIncidente);

            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.FechaHoraModificacion).HasColumnType("datetime");
            entity.Property(e => e.FechaHoraRegistro).HasColumnType("datetime");
            entity.Property(e => e.IdDesignacionTlp).HasColumnName("IdDesignacionTLP");
            entity.Property(e => e.IpModificacion).HasColumnType("datetime");
            entity.Property(e => e.IpRegistro)
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode(false);
            entity.Property(e => e.NivelPrioridad).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Referencias).IsUnicode(false);
            entity.Property(e => e.UsuarioModificacion)
            .HasMaxLength(20)
            .IsUnicode(false);
            entity.Property(e => e.UsuarioRegistro)
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode(false);
            entity.Property(e => e.VulnerabilidadCve)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("VulnerabilidadCVE");

            entity.HasOne(d => d.IdCategoriaIncidenteNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdCategoriaIncidente)
            .HasConstraintName("FK_Incidente_CategoriaIncidente");

            entity.HasOne(d => d.IdCiudadLocalizacionNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdCiudadLocalizacion)
            .HasConstraintName("FK_Incidente_CiudadLocalizacion");

            entity.HasOne(d => d.IdDesignacionTlpNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdDesignacionTlp)
            .HasConstraintName("FK_Incidente_DesignacionTLP");

            entity.HasOne(d => d.IdModoIncidenteNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdModoIncidente)
            .HasConstraintName("FK_Incidente_ModoIncidente");

            entity.HasOne(d => d.IdNivelCriticidadNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdNivelCriticidad)
            .HasConstraintName("FK_Incidente_NivelCriticidad");

            entity.HasOne(d => d.IdNivelImpactoActualNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdNivelImpactoActual)
            .HasConstraintName("FK_Incidente_NivelImpactoActual");

            entity.HasOne(d => d.IdNivelImpactoEsperadoNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdNivelImpactoEsperado)
            .HasConstraintName("FK_Incidente_NivelImpactoEsperado");

            entity.HasOne(d => d.IdPaisInstitucionNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdPaisInstitucion)
            .HasConstraintName("FK_Incidente_PaisInstitucion");

            entity.HasOne(d => d.IdPaisLocalizacionNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdPaisLocalizacion)
            .HasConstraintName("FK_Incidente_PaisLocalizacion");

            entity.HasOne(d => d.IdTipoIncidenteNavigation).WithMany(p => p.Incidente)
            .HasForeignKey(d => d.IdTipoIncidente)
            .HasConstraintName("FK_Incidente_TipoIncidente");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Incidente> entity);
    }
}
