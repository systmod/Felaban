using System;

namespace Common
{
    public interface IAdminUnitEntity
    {
        int IdUnidadAdmin { get; }
        string Codigo { get; }
        string Descripcion { get; }
        string TipoUnidad { get; }
        Guid Token { get; }
    }
}