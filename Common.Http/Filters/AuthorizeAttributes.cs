using Common;
using Common.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TokenAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public TokenPermissionEnum? Permissions { get; private set; } = null;
    public TokenAuthorizeAttribute() : base() { }
    public TokenAuthorizeAttribute(TokenPermissionEnum permissions) : base()
    {
        Permissions = permissions;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (UsuarioDto)context.HttpContext.Items["User"];

        if (user == null)
        {
            // not logged in
            context.Result = new JsonResult(new OperationResult(System.Net.HttpStatusCode.Unauthorized, "No Autorizado", "No tiene acceso a este servicio")) { StatusCode = StatusCodes.Status401Unauthorized };
        }

        var status = context.HttpContext.Items["Status"] as TokenPermissionEnum?;

        if (Permissions != null)
            switch (status)
            {
                case TokenPermissionEnum.ChangePassword:
                    if (Permissions != TokenPermissionEnum.ChangePassword)
                    {
                        context.Result = new JsonResult(new OperationResult(System.Net.HttpStatusCode.PreconditionRequired, "Debe Cambiar la clave para continuar", "Se requiere cambiar la clave del usuario")) { StatusCode = StatusCodes.Status428PreconditionRequired };
                        return;
                    }
                    break;
                case TokenPermissionEnum.Login:
                    if (Permissions != TokenPermissionEnum.Login)
                    {
                        context.Result = new JsonResult(new OperationResult(System.Net.HttpStatusCode.Forbidden, "No Permitido", "El usuario no tiene permisos para ejecutar esta operación.")) { StatusCode = StatusCodes.Status401Unauthorized };
                        return;
                    }
                    break;
                case TokenPermissionEnum.Authorized:
                    var empresa = (EmpresaDto)context.HttpContext.Items["Empresa"];

                    if (empresa == null)
                    {
                        // not logged in
                        context.Result = new JsonResult(new OperationResult(System.Net.HttpStatusCode.Unauthorized, "No Permitido", "El usuario no tiene permisos para ejecutar esta operación.")) { StatusCode = StatusCodes.Status401Unauthorized };
                    }

                    var aplicacion = (IApplicationEntity)context.HttpContext.Items["Aplicacion"];

                    if (aplicacion == null)
                    {
                        // not logged in
                        context.Result = new JsonResult(new OperationResult(System.Net.HttpStatusCode.Unauthorized, "No Autorizado", "No tiene acceso a esta aplicación")) { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                    break;
            }
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var aplicacion = (IApplicationEntity)context.HttpContext.Items["Aplicacion"];
//#if !DEBUG
//                if (aplicacion == null)
//                {
//                    // not logged in
//                    context.Result = new JsonResult(new OperationResult(System.Net.HttpStatusCode.Unauthorized, "Servicio no Autorizado", "La Aplicación no tiene acceso a este servicio!")) { StatusCode = StatusCodes.Status401Unauthorized };
//                }
//#endif
    }
}
