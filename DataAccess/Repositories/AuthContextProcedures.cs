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
    public partial class AuthContext
    {
        private IAuthContextProcedures _procedures;

        public virtual IAuthContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new AuthContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IAuthContextProcedures GetProcedures()
        {
            return Procedures;
        }

        protected void OnModelCreatingGeneratedProcedures(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActualizarCorreoResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<ActualizarPasswordResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<AgregarUsuarioResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<IniciarSesionResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<IniciarSesionOtpResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<LogSesionResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<ObtenerPermisosResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<ObtenerUsuariosResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<ValidateUsernameResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<VerifyUserEmailV2Result>().HasNoKey().ToView(null);
        }
    }

    public partial class AuthContextProcedures : IAuthContextProcedures
    {
        private readonly AuthContext _context;

        public AuthContextProcedures(AuthContext context)
        {
            _context = context;
        }

        public virtual async Task<List<ActualizarCorreoResult>> ActualizarCorreoAsync(string cedula, string correo, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "cedula",
                    Size = 50,
                    Value = cedula ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "correo",
                    Size = 250,
                    Value = correo ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ActualizarCorreoResult>("EXEC @returnValue = [dbo].[ActualizarCorreo] @cedula, @correo", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<ActualizarPasswordResult>> ActualizarPasswordAsync(string Token, string Password, bool? Habilitado, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Token",
                    Size = 1000,
                    Value = Token ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Password",
                    Size = 100,
                    Value = Password ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Habilitado",
                    Value = Habilitado ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ActualizarPasswordResult>("EXEC @returnValue = [dbo].[ActualizarPassword] @Token, @Password, @Habilitado", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<AgregarUsuarioResult>> AgregarUsuarioAsync(string Identificacion, string InicioSesion, string Nombre, string Correo, string Telefono, string Password, int? IdAplicacion, string IdPerfil, int? IdEmpresa, int? IdUnidadAdmin, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Identificacion",
                    Size = 20,
                    Value = Identificacion ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "InicioSesion",
                    Size = 80,
                    Value = InicioSesion ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Nombre",
                    Size = 80,
                    Value = Nombre ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Correo",
                    Size = 100,
                    Value = Correo ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Telefono",
                    Size = 100,
                    Value = Telefono ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Password",
                    Size = 100,
                    Value = Password ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "IdAplicacion",
                    Value = IdAplicacion ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "IdPerfil",
                    Size = 50,
                    Value = IdPerfil ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "IdEmpresa",
                    Value = IdEmpresa ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "IdUnidadAdmin",
                    Value = IdUnidadAdmin ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<AgregarUsuarioResult>("EXEC @returnValue = [dbo].[AgregarUsuario] @Identificacion, @InicioSesion, @Nombre, @Correo, @Telefono, @Password, @IdAplicacion, @IdPerfil, @IdEmpresa, @IdUnidadAdmin", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<IniciarSesionResult>> IniciarSesionAsync(string usuario, string clave, string apptoken, string deviceid, string latitud, string longitud, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "usuario",
                    Size = 508,
                    Value = usuario ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "clave",
                    Size = 100,
                    Value = clave ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "apptoken",
                    Size = 2000,
                    Value = apptoken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "deviceid",
                    Size = 2000,
                    Value = deviceid ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "latitud",
                    Size = 200,
                    Value = latitud ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "longitud",
                    Size = 200,
                    Value = longitud ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<IniciarSesionResult>("EXEC @returnValue = [dbo].[IniciarSesion] @usuario, @clave, @apptoken, @deviceid, @latitud, @longitud", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<IniciarSesionOtpResult>> IniciarSesionOtpAsync(string celular, string apptoken, string deviceid, string latitud, string longitud, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "celular",
                    Size = 100,
                    Value = celular ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "apptoken",
                    Size = 2000,
                    Value = apptoken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "deviceid",
                    Size = 2000,
                    Value = deviceid ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "latitud",
                    Size = 200,
                    Value = latitud ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "longitud",
                    Size = 200,
                    Value = longitud ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<IniciarSesionOtpResult>("EXEC @returnValue = [dbo].[IniciarSesionOtp] @celular, @apptoken, @deviceid, @latitud, @longitud", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<int> LogAuditAsync(string InicioSesion, string IdUsuario, int? IdAplicacion, string Browser, string IpAddress, string Exception, int? ExecutionDuration, DateTime? ExecutionDate, string MethodName, string Parameters, string ServiceName, string ExceptionMessage, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "InicioSesion",
                    Size = 200,
                    Value = InicioSesion ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "IdUsuario",
                    Size = 50,
                    Value = IdUsuario ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "IdAplicacion",
                    Value = IdAplicacion ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "Browser",
                    Size = 1024,
                    Value = Browser ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "IpAddress",
                    Size = 128,
                    Value = IpAddress ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Exception",
                    Size = 4000,
                    Value = Exception ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ExecutionDuration",
                    Value = ExecutionDuration ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "ExecutionDate",
                    Value = ExecutionDate ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "MethodName",
                    Size = 256,
                    Value = MethodName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Parameters",
                    Size = 1024,
                    Value = Parameters ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ServiceName",
                    Size = 256,
                    Value = ServiceName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ExceptionMessage",
                    Size = 1024,
                    Value = ExceptionMessage ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [dbo].[LogAudit] @InicioSesion, @IdUsuario, @IdAplicacion, @Browser, @IpAddress, @Exception, @ExecutionDuration, @ExecutionDate, @MethodName, @Parameters, @ServiceName, @ExceptionMessage", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<LogSesionResult>> LogSesionAsync(string idusuario, string iniciosesion, long? idaplicacion, bool? exitoso, string mensaje, string error, string deviceid, string latitud, string longitud, int? estado, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "idusuario",
                    Size = 50,
                    Value = idusuario ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "iniciosesion",
                    Size = 100,
                    Value = iniciosesion ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "idaplicacion",
                    Value = idaplicacion ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.BigInt,
                },
                new SqlParameter
                {
                    ParameterName = "exitoso",
                    Value = exitoso ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "mensaje",
                    Size = 2000,
                    Value = mensaje ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "error",
                    Size = 2000,
                    Value = error ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "deviceid",
                    Size = 2000,
                    Value = deviceid ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "latitud",
                    Size = 200,
                    Value = latitud ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "longitud",
                    Size = 200,
                    Value = longitud ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "estado",
                    Value = estado ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<LogSesionResult>("EXEC @returnValue = [dbo].[LogSesion] @idusuario, @iniciosesion, @idaplicacion, @exitoso, @mensaje, @error, @deviceid, @latitud, @longitud, @estado", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<ObtenerPermisosResult>> ObtenerPermisosAsync(string appToken, string userToken, string companyToken, string localToken, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "appToken",
                    Size = 50,
                    Value = appToken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "userToken",
                    Size = 50,
                    Value = userToken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "companyToken",
                    Size = 50,
                    Value = companyToken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "localToken",
                    Size = 50,
                    Value = localToken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ObtenerPermisosResult>("EXEC @returnValue = [dbo].[ObtenerPermisos] @appToken, @userToken, @companyToken, @localToken", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<ObtenerUsuariosResult>> ObtenerUsuariosAsync(Guid? appToken, Guid? companyToken, string term, Guid? idPerfil, Guid? unitToken, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "appToken",
                    Value = appToken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "companyToken",
                    Value = companyToken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "term",
                    Size = 1000,
                    Value = term ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "idPerfil",
                    Value = idPerfil ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "unitToken",
                    Value = unitToken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ObtenerUsuariosResult>("EXEC @returnValue = [dbo].[ObtenerUsuarios] @appToken, @companyToken, @term, @idPerfil, @unitToken", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<ValidateUsernameResult>> ValidateUsernameAsync(string username, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "username",
                    Size = 100,
                    Value = username ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ValidateUsernameResult>("EXEC @returnValue = [dbo].[ValidateUsername] @username", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<VerifyUserEmailV2Result>> VerifyUserEmailV2Async(long? Id, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Id",
                    Value = Id ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.BigInt,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<VerifyUserEmailV2Result>("EXEC @returnValue = [dbo].[VerifyUserEmailV2] @Id", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
