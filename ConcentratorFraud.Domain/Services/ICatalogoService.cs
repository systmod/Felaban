using Common;
using ConcentratorFraud.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Domain.Services
{
    public interface ICatalogoService
    {
        Task<IOperationResultList<TipoIncidenteDto>> GetTiposIncidentes(string term = default, int page = 1, int? pageSize = default);
        Task<IOperationResultList<TipoActivoAfectadoDto>> GetTiposActivosAfectados(string term = default, int page = 1, int? pageSize = default);
        Task<IOperationResultList<TipoActivoObjectivoDto>> GetTiposActivosObjectivo(string term = default, int page = 1, int? pageSize = default);
        Task<IOperationResultList<TipoActivoNoAfectadoDto>> GetTiposActivosNoAfectado(string term = default, int page = 1, int? pageSize = default);
        Task<IOperationResultList<CategoriaIncidenteDto>> GetCategoriasIncidentes(string term = default, int page = 1, int? pageSize = default);
        Task<IOperationResultList<ModoIncidenteDto>> GetModosIncidentes(string term = default, int page = 1, int? pageSize = default);
        Task<IOperationResultList<NivelCriticidadDto>> GetNivelesCriticidad(string term = default, int page = 1, int? pageSize = default);
        Task<IOperationResultList<NivelImpactoActualDto>> GetNivelesImpactoActual(string term = default, int page = 1, int? pageSize = default);
        Task<IOperationResultList<NivelImpactoEsperadoDto>> GetNivelesImpactoEsperado(string term = default, int page = 1, int? pageSize = default);
    }
}
