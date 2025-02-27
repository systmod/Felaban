﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public partial interface IAuthContextProcedures
    {
        Task<List<ActualizarCorreoResult>> ActualizarCorreoAsync(string cedula, string correo, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<ActualizarPasswordResult>> ActualizarPasswordAsync(string Token, string Password, bool? Habilitado, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<AgregarUsuarioResult>> AgregarUsuarioAsync(string Identificacion, string InicioSesion, string Nombre, string Correo, string Telefono, string Password, int? IdAplicacion, string IdPerfil, int? IdEmpresa, int? IdUnidadAdmin, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<IniciarSesionResult>> IniciarSesionAsync(string usuario, string clave, string apptoken, string deviceid, string latitud, string longitud, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<IniciarSesionOtpResult>> IniciarSesionOtpAsync(string celular, string apptoken, string deviceid, string latitud, string longitud, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> LogAuditAsync(string InicioSesion, string IdUsuario, int? IdAplicacion, string Browser, string IpAddress, string Exception, int? ExecutionDuration, DateTime? ExecutionDate, string MethodName, string Parameters, string ServiceName, string ExceptionMessage, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<LogSesionResult>> LogSesionAsync(string idusuario, string iniciosesion, long? idaplicacion, bool? exitoso, string mensaje, string error, string deviceid, string latitud, string longitud, int? estado, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<ObtenerPermisosResult>> ObtenerPermisosAsync(string appToken, string userToken, string companyToken, string localToken, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<ObtenerUsuariosResult>> ObtenerUsuariosAsync(Guid? appToken, Guid? companyToken, string term, Guid? idPerfil, Guid? unitToken, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<ValidateUsernameResult>> ValidateUsernameAsync(string username, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<VerifyUserEmailV2Result>> VerifyUserEmailV2Async(long? Id, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
