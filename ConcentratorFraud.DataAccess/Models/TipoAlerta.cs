﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ConcentratorFraud.DataAccess.Models;

public partial class TipoAlerta
{
    public int IdTipoAlerta { get; set; }

    public string Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<RiesgoIndicador> RiesgoIndicador { get; set; } = new List<RiesgoIndicador>();
}