﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public partial class IniciarSesionOtpResult
    {
        public bool? Success { get; set; }
        public int? StatusCode { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string IdUsuario { get; set; }
        public string Identificacion { get; set; }
        public string Ruc { get; set; }
        public string InicioSesion { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public Guid Token { get; set; }
        public bool IdEstado { get; set; }
    }
}
