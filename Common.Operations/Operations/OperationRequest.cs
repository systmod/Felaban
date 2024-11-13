using Common.Domain.Models;
using System;

namespace Common
{
    public class OperationRequest<T> : IOperationRequest<T>
    {
        public string Ip { get; set; }

        public DateTime Fecha { get; set; }

        public T Data { get; set; }

        public string InicioSesion => Usuario.InicioSesion;

        public ICompanyEntity Empresa { get; set; }

        public IUserEntity Usuario { get; set; }

        public IAdminUnitEntity Local => Usuario.Local;

        public IApplicationEntity Aplicacion { get; set; }

        public OperationRequest(T entidad, ICompanyEntity empresa, IUserEntity usuario = default, IApplicationEntity aplicacion = default, object ipAddress = default, DateTime? fecha = default)
        {
            Empresa = empresa;
            Usuario = usuario ?? new UsuarioDto();
            Aplicacion = aplicacion;
            Ip = $"{ipAddress}";
            Fecha = fecha ?? DateTime.Now;
            Data = entidad;
        }

    }
}
