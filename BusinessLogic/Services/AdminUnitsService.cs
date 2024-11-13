using AutoMapper;
using Common;
using Common.Domain.Services;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AdminUnitsService: IAdminUnitsService
    {
        private readonly AuthContext _db;
        private readonly IAuthContextProcedures _procedures;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<Aplicacion> _aplicacion;
        public AdminUnitsService()
        {
            
        }

    }
}
