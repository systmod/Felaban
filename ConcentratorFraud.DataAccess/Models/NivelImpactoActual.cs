﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ConcentratorFraud.DataAccess.Models;

public partial class NivelImpactoActual
{
    public int IdNivelImpactoActual { get; set; }

    public string Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Incidente> Incidente { get; set; } = new List<Incidente>();
}