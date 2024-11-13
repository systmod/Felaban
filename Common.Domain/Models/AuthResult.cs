using System.Net;

namespace Common.Domain.Models
{
    public class AuthResult : IOperationResult<UsuarioDto>
    {
        public UsuarioDto Result { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
    }
}
