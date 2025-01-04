using Common;
using Common.Http;
using ConcentratorFraud.Domain.Model;
using ConcentratorFraud.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConcentratorFraud.API.Controllers
{

    //[TokenAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoController : ApiControllerBase
    {
        private IConfiguration _config;
        private ICatalogoService _catalogo;
        private ILogger<CatalogoController> _logger;

        public CatalogoController(IConfiguration config,
            ICatalogoService catalogo,
            ILogger<CatalogoController> logger)
        {
            _config = config;
            _catalogo = catalogo;
            _logger = logger;
        }
                
        [HttpGet, Route("tipos-incidentes")]
        [ProducesResponseType(typeof(IOperationResultList<TipoIncidenteDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetTiposIncidentes(string? term, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _catalogo.GetTiposIncidentes(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpGet, Route("tipos-activos-afectados")]
        [ProducesResponseType(typeof(IOperationResultList<TipoActivoAfectadoDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetTiposActivosAfectados(string? term, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _catalogo.GetTiposActivosAfectados(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpGet, Route("tipos-activos-objectivo")]
        [ProducesResponseType(typeof(IOperationResultList<TipoActivoObjectivoDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetTiposActivosObjectivo(string? term, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _catalogo.GetTiposActivosObjectivo(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpGet, Route("tipos-activos-no-afectados")]
        [ProducesResponseType(typeof(IOperationResultList<TipoActivoNoAfectadoDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetTiposActivosNoAfectado(string? term, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _catalogo.GetTiposActivosNoAfectado(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpGet, Route("categorias-incidentes")]
        [ProducesResponseType(typeof(IOperationResultList<CategoriaIncidenteDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetCategoriasIncidentes(string? term, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _catalogo.GetCategoriasIncidentes(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpGet, Route("modos-incidentes")]
        [ProducesResponseType(typeof(IOperationResultList<ModoIncidenteDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetModosIncidentes(string? term, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _catalogo.GetModosIncidentes(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpGet, Route("niveles-criticidad")]
        [ProducesResponseType(typeof(IOperationResultList<NivelCriticidadDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetNivelesCriticidad(string? term, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _catalogo.GetNivelesCriticidad(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpGet, Route("niveles-impacto-actual")]
        [ProducesResponseType(typeof(IOperationResultList<NivelImpactoActualDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetNivelesImpactoActual(string? term, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _catalogo.GetNivelesImpactoActual(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpGet, Route("niveles-impacto-esperado")]
        [ProducesResponseType(typeof(IOperationResultList<NivelImpactoEsperadoDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetNivelesImpactoEsperado(string? term, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _catalogo.GetNivelesImpactoEsperado(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }
    }
}
