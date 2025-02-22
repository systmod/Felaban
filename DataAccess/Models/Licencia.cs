﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Licencia
{
    public Guid IdLicencia { get; set; }

    public int IdProducto { get; set; }

    public string Codigo { get; set; }

    public string Descripcion { get; set; }

    public decimal Precio { get; set; }

    public bool Oferta { get; set; }

    public bool Activo { get; set; }

    public virtual Producto IdProductoNavigation { get; set; }

    public virtual ICollection<ProductoEmpresa> ProductoEmpresa { get; set; } = new List<ProductoEmpresa>();
}