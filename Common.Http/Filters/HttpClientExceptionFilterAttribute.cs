using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

public class HttpClientExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        try
        {
            var result = new OperationResult(context.Exception);
            context.Result = new JsonResult(result);
            context.HttpContext.Response.StatusCode = (int)result.StatusCode;
        }
        catch (Exception ex)
        {
            var result = new OperationResult(ex);
            context.Result = new JsonResult(result);
            context.HttpContext.Response.StatusCode = (int)result.StatusCode;
        }

        base.OnException(context);
    }


}