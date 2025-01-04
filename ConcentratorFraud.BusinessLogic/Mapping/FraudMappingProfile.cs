using AutoMapper;
using Common;
using Common.Domain.Models;
using ConcentratorFraud.DataAccess.Models;
using ConcentratorFraud.Domain.Model;
using ConcentratorFraud.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.BusinessLogic.Mapping
{
    public class FraudMappingProfile : Profile
    {
        public FraudMappingProfile()
        {
            CreateMap<Incidente, IncidenteDto>().ReverseMap();
            CreateMap<Incidente, IncidenteRequest>().ReverseMap();

            CreateMap<TipoIncidente, TipoIncidenteDto>().ReverseMap();

            CreateMap<TipoActivoAfectado, TipoActivoAfectadoDto>().ReverseMap();

            CreateMap<TipoActivoObjectivo, TipoActivoObjectivoDto>().ReverseMap();

            CreateMap<TipoActivoNoAfectado, TipoActivoNoAfectadoDto>().ReverseMap();

            CreateMap<CategoriaIncidente, CategoriaIncidenteDto>().ReverseMap();

            CreateMap<ModoIncidente, ModoIncidenteDto>().ReverseMap();
            
            CreateMap<NivelCriticidad, NivelCriticidadDto>().ReverseMap();
            
            CreateMap<NivelImpactoActual, NivelImpactoActualDto>().ReverseMap();
            
            CreateMap<NivelImpactoEsperado, NivelImpactoEsperadoDto>().ReverseMap();

        }
    }
}
