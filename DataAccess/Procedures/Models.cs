using Common;
using System.Net;

namespace DataAccess.Models
{
    public partial class ActualizarCorreoResult : IOperationResult
    {
        HttpStatusCode IOperationResult.StatusCode { get => (HttpStatusCode)StatusCode; set => StatusCode = (int)value; }

        bool IOperationResult.Success => this.Success ?? false;
    }

    public partial class ActualizarPasswordResult : IOperationResult
    {
        HttpStatusCode IOperationResult.StatusCode { get => (HttpStatusCode)StatusCode; set => StatusCode = (int)value; }

        bool IOperationResult.Success => this.Success ?? false;
    }

    public partial class AgregarUsuarioResult : IOperationResult
    {
        HttpStatusCode IOperationResult.StatusCode { get => (HttpStatusCode)StatusCode; set => StatusCode = (int)value; }

        bool IOperationResult.Success => this.Success ?? false;
    }

    public partial class IniciarSesionResult : IOperationResult
    {
        HttpStatusCode IOperationResult.StatusCode { get => (HttpStatusCode)StatusCode; set => StatusCode = (int)value; }

        bool IOperationResult.Success => this.Success ?? false;
    }

}
