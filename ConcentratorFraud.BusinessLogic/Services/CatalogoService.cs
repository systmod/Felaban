using AutoMapper;
using Common;
using ConcentratorFraud.DataAccess.Models;
using ConcentratorFraud.Domain.Model;
using ConcentratorFraud.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.BusinessLogic.Services
{
    public class CatalogoService: ICatalogoService
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepository<TipoIncidente> _tipoIncidente;
        private readonly IEntityRepository<TipoActivoAfectado> _tipoActivoAfectado;
        private readonly IEntityRepository<TipoActivoObjectivo> _tipoActivoObjectivo;
        private readonly IEntityRepository<TipoActivoNoAfectado> _tipoActivoNoAfectado;
        private readonly IEntityRepository<CategoriaIncidente> _categoriaIncidente;
        private readonly IEntityRepository<ModoIncidente> _modoIncidente;
        private readonly IEntityRepository<NivelCriticidad> _nivelCriticidad;
        private readonly IEntityRepository<NivelImpactoActual> _nivelImpactoActual;
        private readonly IEntityRepository<NivelImpactoEsperado> _nivelImpactoEsperado;

        public CatalogoService(
            IMapper mapper,
            IEntityRepository<TipoIncidente> tipoIncidente,
            IEntityRepository<TipoActivoAfectado> tipoActivoAfectado,
            IEntityRepository<TipoActivoObjectivo> tipoActivoObjectivo,
            IEntityRepository<TipoActivoNoAfectado> tipoActivoNoAfectado,
            IEntityRepository<CategoriaIncidente> categoriaIncidente,
            IEntityRepository<ModoIncidente> modoIncidente,
            IEntityRepository<NivelCriticidad> nivelCriticidad,
            IEntityRepository<NivelImpactoActual> nivelImpactoActual,
            IEntityRepository<NivelImpactoEsperado> nivelImpactoEsperado
            )
        {
            _mapper = mapper;
            _tipoIncidente = tipoIncidente;
            _tipoActivoAfectado = tipoActivoAfectado;
            _tipoActivoObjectivo = tipoActivoObjectivo;
            _tipoActivoNoAfectado = tipoActivoNoAfectado;
            _categoriaIncidente = categoriaIncidente;
            _modoIncidente = modoIncidente;
            _nivelCriticidad = nivelCriticidad;
            _nivelImpactoActual = nivelImpactoActual;
            _nivelImpactoEsperado = nivelImpactoEsperado;
        }

        public async Task<IOperationResultList<TipoIncidenteDto>> GetTiposIncidentes(string term = default, int page = 1, int? pageSize = default)
        {
            try
            { 
                var tipos = _tipoIncidente.Search(x => x.Activo && (term == null || x.Descripcion.Contains(term)))
                                .AsQueryable(); // Cargo el resto de la información requerida;

            return await tipos.ToResultListAsync<TipoIncidente, TipoIncidenteDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<TipoIncidenteDto>();
            }
        }
        public async Task<IOperationResultList<TipoActivoAfectadoDto>> GetTiposActivosAfectados(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var tipos = _tipoActivoAfectado.Search(x => x.Activo && (term == null || x.Descripcion.Contains(term)))
                                .AsQueryable(); // Cargo el resto de la información requerida;

                return await tipos.ToResultListAsync<TipoActivoAfectado, TipoActivoAfectadoDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<TipoActivoAfectadoDto>();
            }
        }
        public async Task<IOperationResultList<TipoActivoObjectivoDto>> GetTiposActivosObjectivo(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var tipos = _tipoActivoObjectivo.Search(x => x.Activo && (term == null || x.Descripcion.Contains(term)))
                                .AsQueryable(); // Cargo el resto de la información requerida;

                return await tipos.ToResultListAsync<TipoActivoObjectivo, TipoActivoObjectivoDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<TipoActivoObjectivoDto>();
            }
        }
        public async Task<IOperationResultList<TipoActivoNoAfectadoDto>> GetTiposActivosNoAfectado(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var tipos = _tipoActivoNoAfectado.Search(x => x.Activo && (term == null || x.Descripcion.Contains(term)))
                                .AsQueryable(); // Cargo el resto de la información requerida;

                return await tipos.ToResultListAsync<TipoActivoNoAfectado, TipoActivoNoAfectadoDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<TipoActivoNoAfectadoDto>();
            }
        }
        public async Task<IOperationResultList<CategoriaIncidenteDto>> GetCategoriasIncidentes(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var tipos = _categoriaIncidente.Search(x => x.Activo && (term == null || x.Descripcion.Contains(term)))
                                .AsQueryable(); // Cargo el resto de la información requerida;

                return await tipos.ToResultListAsync<CategoriaIncidente, CategoriaIncidenteDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<CategoriaIncidenteDto>();
            }
        }
        public async Task<IOperationResultList<ModoIncidenteDto>> GetModosIncidentes(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var tipos = _modoIncidente.Search(x => x.Activo == true && (term == null || x.Descripcion.Contains(term)))
                                .AsQueryable(); // Cargo el resto de la información requerida;

                return await tipos.ToResultListAsync<ModoIncidente, ModoIncidenteDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<ModoIncidenteDto>();
            }
        }
        public async Task<IOperationResultList<NivelCriticidadDto>> GetNivelesCriticidad(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var tipos = _nivelCriticidad.Search(x => x.Activo == true && (term == null || x.Descripcion.Contains(term)))
                                .AsQueryable(); // Cargo el resto de la información requerida;

                return await tipos.ToResultListAsync<NivelCriticidad, NivelCriticidadDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<NivelCriticidadDto>();
            }
        }
        public async Task<IOperationResultList<NivelImpactoActualDto>> GetNivelesImpactoActual(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var tipos = _nivelImpactoActual.Search(x => x.Activo == true && (term == null || x.Descripcion.Contains(term)))
                                .AsQueryable(); // Cargo el resto de la información requerida;

                return await tipos.ToResultListAsync<NivelImpactoActual, NivelImpactoActualDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<NivelImpactoActualDto>();
            }
        }
        public async Task<IOperationResultList<NivelImpactoEsperadoDto>> GetNivelesImpactoEsperado(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var tipos = _nivelImpactoEsperado.Search(x => x.Activo == true && (term == null || x.Descripcion.Contains(term)))
                                .AsQueryable(); // Cargo el resto de la información requerida;

                return await tipos.ToResultListAsync<NivelImpactoEsperado, NivelImpactoEsperadoDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<NivelImpactoEsperadoDto>();
            }
        }
    }
}
