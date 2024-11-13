using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Models
{
    public class FindUsersRequest
    {
        public string Term { get; set; }
        public Guid? Perfil { get; set; }
        public Guid? Unidad { get; set; }
    }
}
