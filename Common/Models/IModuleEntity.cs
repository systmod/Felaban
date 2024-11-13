using System.Collections.Generic;

namespace Common
{
    public interface IModuleEntity
    {
        string Codigo { get; }
        string Descripcion { get; }
        string Icono { get; }   
        List<IOptionEntity> Opciones { get; }
    }
}