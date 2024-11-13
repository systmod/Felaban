using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Models
{
    public class AuditLogsRequest
    {        
        public string InicioSesion { get; set; }
        public string IdUsuario { get; set; }
        public int? IdAplicacion { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public string Exception { get; set; }
        public int? ExecutionDuration { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public string ServiceName { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
