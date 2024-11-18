using AutoMapper;
using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Common.Storage.Handlers;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class CompanyService: ICompanyService
    {
        private readonly AuthContext _db;
        private readonly IAuthContextProcedures _procedures;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<Aplicacion> _aplicacion;
        private readonly IEntityRepository<Usuario> _usuario;
        private readonly FileHandler _fileHandler;
        public CompanyService(
                            AuthContext db,
                IAuthContextProcedures proc,
                IMapper mapper,
                IEntityRepository<Aplicacion> aplicacion,
                IEntityRepository<Usuario> usuario,
                FileHandler fileHandler
            )
        {
            _db = db;
            _procedures = proc;
            _mapper = mapper;
            _aplicacion = aplicacion;
            _usuario = usuario;
            _fileHandler = fileHandler;
        }

        //public async Task<IOperationResult> SaveCompany(IOperationRequest<EmpresaRequest> model)
        //{
        //    try
        //    {
        //        var dat = model.Data;

        //        await _procedures.AgregarEmpresaAsync(dat.IdEmpresaD, dat.IdEmpresaH, dat.Ruc, dat.RazonSocial, dat.NombreComercial, dat.CodigoSbs, dat.Pais, dat.Moneda, "URLLOGO",
        //                                             dat.Siglas, dat.Celular, dat.UTC, dat.Direccion, dat.Email);

        //        return new OperationResult(HttpStatusCode.OK);
        //    }
        //    catch (Exception ex)
        //    {
        //        return await ex.ToResultAsync();
        //    }
        //}

        public async Task<IOperationResult<EmpresaDto>> UpdateEmpresa(IOperationRequest<EmpresaRequest> request)
        {
            var empresa = _db.Empresa.FirstOrDefault(x => x.IdEmpresa == request.Empresa.IdEmpresa);

            if (empresa == null)
            {
                return new OperationResult<EmpresaDto>(HttpStatusCode.NotFound, "empresa no encontrada");
            }

            empresa = _mapper.Map<Empresa>(empresa, request.Data);

            if (!string.IsNullOrEmpty(request.Data.Logo))
            {
                var url = await UploadFileAuth(request.Data.Logo, $"LOGO", empresa.IdEmpresa);
                empresa.UrlLogo = url.Result;
            }

            await _db.Empresa.UpdateAsync(empresa);
            await _db.SaveAsync(request);

            var result = _mapper.Map<EmpresaDto>(empresa);

            return await result.ToResultAsync();
        }

        public async Task<IOperationResult<EmpresaDto>> GetEmpresaById(int id)
        {
            var result = _db.Empresa.FirstOrDefault(x => x.IdEmpresa == id);
            return await _mapper.Map<EmpresaDto>(result).ToResultAsync();
        }



        private async Task<IOperationResult<string>> UploadFileAuth(string imagen, string id, int idEmpresa)
        {
            var file = Convert.FromBase64String(imagen);
            var path = $"{idEmpresa}/{id}-v{DateTime.Now.ToFileTime()}.jpg".ToLower();
            var stream = new MemoryStream(file);

            var result = await _fileHandler.UploadAsync(path, stream);

            var url = await _fileHandler.GetFile(path);
            return await url.Result.ToResultAsync();

        }
    }
}
