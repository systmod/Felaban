﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ConcentratorFraud.DataAccess.Models;
using ConcentratorFraud.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ConcentratorFraud.DataAccess.Repositories.Configurations
{
    public partial class TipoActivoNoAfectadoConfiguration : IEntityTypeConfiguration<TipoActivoNoAfectado>
    {
        public void Configure(EntityTypeBuilder<TipoActivoNoAfectado> entity)
        {
            entity.HasKey(e => e.IdTipoActivoNoAfectado);

            entity.Property(e => e.IdTipoActivoNoAfectado).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TipoActivoNoAfectado> entity);
    }
}
