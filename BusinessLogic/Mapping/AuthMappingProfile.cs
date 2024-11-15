using AutoMapper;
using Common;
using Common.Domain.Models;
using ConcentratorFraud.Felaban.Auth.Domain.Request;
using DataAccess.Models;

namespace BusinessLogic.Mapping
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            //CreateMap<TokenPermissionEnum, short>();
            CreateMap<Usuario, UsuarioDto>()
                .ReverseMap();

            CreateMap<UnidadAdmin, UnidadAdminDto>()
                .ForMember(member => member.TipoUnidad, opts => opts.MapFrom(x => x.IdTipoUnidadAdminNavigation.Descripcion))
                .ReverseMap();
            
            CreateMap<Perfil, PerfilDto>().ReverseMap();
            CreateMap<Perfil, PerfilRequest>().ReverseMap();

            CreateMap<Modulo, ModuloDto>().ReverseMap();
            CreateMap<Modulo, ModuloRequest>().ReverseMap();

            CreateMap<Opcion, OpcionDto>().ReverseMap();

            CreateMap<Empresa, EmpresaDto>().ReverseMap();
            CreateMap<Empresa, EmpresaRequest>().ReverseMap();

            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Producto, ProductoRequest>().ReverseMap();

            CreateMap<Aplicacion, AplicacionDto>()
                .ForMember(member => member.Producto, opts => opts.MapFrom(x => x.IdProductoNavigation))
                .ReverseMap();
            CreateMap<Aplicacion, AplicacionRequest>()    
                .ReverseMap();

            CreateMap<IniciarSesionResult, UsuarioDto>().ReverseMap();
            CreateMap<AgregarUsuarioResult, UsuarioDto>().ReverseMap();
            CreateMap<Perfil, RolesDto>()
                .ForMember(member => member.EsAdicional, opts => opts.MapFrom(x => x.IdTipoPerfilNavigation.EsAdicional)) 
                .ForMember(member => member.AccesoTotal, opts => opts.MapFrom(x => x.IdTipoPerfilNavigation.AccesoTotal))  
                .ReverseMap();

            CreateMap<Opcion, OptionDto>().ReverseMap();
            CreateMap<ObtenerPermisosResult, OpcionDto>().ReverseMap();

            CreateMap<AssignUnitRequest, UsuarioUnidad>().ReverseMap();
            CreateMap<UsuarioUnidad, UsuarioUnidadDto>()
                .ForMember(member => member.Perfil, opts => opts.MapFrom(x => x.IdPerfilNavigation.Descripcion))
                .ReverseMap();

            CreateMap<UnidadAdmin, UsuarioUnidadDto>()
                .ForMember(member => member.TipoUnidad, opts => opts.MapFrom(x => x.IdTipoUnidadAdminNavigation.Descripcion))
                .ReverseMap();

            CreateMap<AssignUnitRequest, UsuarioUnidad>().ReverseMap();
            CreateMap<ObtenerUsuariosResult, UsuarioDto>().ReverseMap();
            CreateMap<ActualizarPasswordResult, UsuarioDto>().ReverseMap();

            CreateMap<UserUpdateRequest, Usuario>()
                .ForMember(member => member.Nombre, opts => opts.MapFrom(x => x.Nombres))
                .ForMember(member => member.Telefono, opts => opts.MapFrom(x => x.Celular))
                .ReverseMap();


            CreateMap<TipoPerfil, TipoPerfilDto>().ReverseMap();

            CreateMap<ProfileDetailRequest, DetallePerfil>();
            CreateMap<DetallePerfil, ProfileDetailDto>()
                .ForMember(member => member.Opcion, opts => opts.MapFrom(x => x.IdOpcionNavigation.Nombre))
                .ReverseMap();

            CreateMap<Custom_AdminUnitsByCompanyResult, UnidadAdminDto>()
                .ReverseMap();

            CreateMap<UserMailVerify, UserMailVerifyDto>().ReverseMap();
        }
    }

}
