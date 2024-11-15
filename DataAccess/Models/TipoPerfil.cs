﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class TipoPerfil
{
    public int IdTipoPerfil { get; set; }

    public string Codigo { get; set; }

    public string Descripcion { get; set; }

    public bool AccesoTotal { get; set; }

    public bool EsAdicional { get; set; }

    public string IpIngreso { get; set; }

    public string UsuarioIngreso { get; set; }

    public DateTime FechaIngreso { get; set; }

    public string IpModificacion { get; set; }

    public string UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public short IdEstado { get; set; }

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();
}