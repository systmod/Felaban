﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ConcentratorFraud.DataAccess.Models;
using ConcentratorFraud.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ConcentratorFraud.DataAccess.Repositories.Configurations
{
    public partial class PaisInstitucionConfiguration : IEntityTypeConfiguration<PaisInstitucion>
    {
        public void Configure(EntityTypeBuilder<PaisInstitucion> entity)
        {
            entity.HasKey(e => e.IdPaisInstitucion);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PaisInstitucion> entity);
    }
}
