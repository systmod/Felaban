﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ConcentratorFraud.DataAccess.Models;

public partial class RiesgoIndicador
{
    public int IdRiesgoIndicador { get; set; }

    public int? IdIncidente { get; set; }

    public int? IdTipoAlerta { get; set; }

    public string Mensaje { get; set; }

    public bool Activo { get; set; }

    public virtual Incidente IdIncidenteNavigation { get; set; }

    public virtual TipoAlerta IdTipoAlertaNavigation { get; set; }
}