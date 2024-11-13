using System;

namespace Common
{
    public interface IOperationRequest
    {
        string Ip { get; }

        DateTime Fecha { get; }

        string InicioSesion { get; }

        IUserEntity Usuario { get; }

        ICompanyEntity Empresa { get; }

        IApplicationEntity Aplicacion { get; }

        IAdminUnitEntity Local { get; }
    }

    public interface IOperationRequest<T> : IOperationRequest
    {
        public T Data { get; set; }
    }
}
