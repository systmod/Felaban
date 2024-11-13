using System;
using System.Collections.Generic;

namespace Common
{
    public interface IProfileEntity
    {
        string Descripcion { get; set; }
        Guid IdPerfil { get; set; }
        List<IModuleEntity> Permisos { get; set; }
    }
}
