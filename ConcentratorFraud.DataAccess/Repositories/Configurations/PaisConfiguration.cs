﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ConcentratorFraud.DataAccess.Models;
using ConcentratorFraud.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ConcentratorFraud.DataAccess.Repositories.Configurations
{
    public partial class PaisConfiguration : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> entity)
        {
            entity.HasKey(e => e.IdPais);

            entity.Property(e => e.Descripcion)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Pais> entity);
    }
}
