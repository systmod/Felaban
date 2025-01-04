﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ConcentratorFraud.DataAccess.Models;
using ConcentratorFraud.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ConcentratorFraud.DataAccess.Repositories.Configurations
{
    public partial class TipoActivoAfectadoConfiguration : IEntityTypeConfiguration<TipoActivoAfectado>
    {
        public void Configure(EntityTypeBuilder<TipoActivoAfectado> entity)
        {
            entity.HasKey(e => e.IdTipoActivoAfectado);

            entity.Property(e => e.IdTipoActivoAfectado).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TipoActivoAfectado> entity);
    }
}
