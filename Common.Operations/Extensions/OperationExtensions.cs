using AutoMapper;
using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace System
{
    public static class OperationExtensions
    {
        static IMapper _mapper;

        public static void Initialize(IServiceProvider services)
        {
            _mapper = (IMapper)services.GetService(typeof(IMapper));
        }

        public static IOperationRequest<T> ToRequest<T>(this T entity, IOperationRequest request)
        {
            return new OperationRequest<T>(entity, request.Empresa, request.Usuario, request.Aplicacion, request.Ip, request.Fecha);
        }

        public static IOperationRequest<T> ToRequest<T>(this T entity, ICompanyEntity empresa, IUserEntity usuario, IApplicationEntity aplicacion, IPAddress ip, DateTime? fecha = default)
        {
            return new OperationRequest<T>(entity, empresa, usuario, aplicacion, ip, fecha ?? DateTime.Now);
        }

        public static IOperationResultList<T> ToResultList<T>(this IEnumerable<T> entity,
                        int pageNumber = 1,
                        int pageSize = 10,
                        int count = 0)
        {
            return new OperationResultList<T>(HttpStatusCode.OK, default, entity, pageNumber, pageSize, count);
        }

        public static IOperationResultList<T> ToResultList<T>(this Exception ex, IEnumerable<T> entity = null)
        {
            return new OperationResultList<T>(ex, entity);
        }

        public static IOperationResultList<T> ToResultList<T>(this IEnumerable<T> entity,
                        HttpStatusCode status,
                        string message = default,
                        int pageNumber = 1,
                        int? pageSize = default,
                        int? count = default
                        )
        {
            return new OperationResultList<T>(status, message, entity, pageNumber, pageSize, count);
        }

        public static IOperationResultList<TResult> ToResultList<TQuery, TResult>(this IQueryable<TQuery> entity,
                            int pageNumber = 1,
                            int? pageSize = default)
        {
            var query = entity;
            var count = query.Count();

            if (pageSize > 0)
            {
                if (pageNumber > 1)
                {
                    query = query.Skip(pageSize.Value * (pageNumber - 1));
                }

                query = query.Take(pageSize.Value);
            }

            var result = query.Select(x => _mapper.Map<TResult>(x));

            return new OperationResultList<TResult>(HttpStatusCode.OK, default, result, pageNumber, pageSize, count);

        }

        public static async Task<IOperationResultList<TResult>> ToResultListAsync<TQuery, TResult>(this IOrderedQueryable<TQuery> entity,
                        int pageNumber = 1,
                        int? pageSize = default)
        {
            return await Task.FromResult(entity.ToResultList<TQuery, TResult>(pageNumber, pageSize));
        }

        public static async Task<IOperationResultList<TResult>> ToResultListAsync<TQuery, TResult>(this IQueryable<TQuery> entity,
                        int pageNumber = 1,
                        int? pageSize = default)
        {
            return await Task.FromResult(entity.ToResultList<TQuery, TResult>(pageNumber, pageSize));
        }

        public static async Task<IOperationResultList<T>> ToResultListAsync<T>(this IEnumerable<T> entity,
                        int pageNumber = 1,
                        int? pageSize = default,
                        int? count = default)
        {
            return await Task.FromResult(new OperationResultList<T>(HttpStatusCode.OK, default, entity, pageNumber, pageSize, count));
        }

        public static async Task<IOperationResultList<T>> ToResultListAsync<T>(this IEnumerable<T> entity,
                        HttpStatusCode status,
                        string message = default,
                        int pageNumber = 1,
                        int? pageSize = default,
                        int? count = default)
        {
            return await Task.FromResult(new OperationResultList<T>(status, message, entity, pageNumber, pageSize, count));
        }


        public static IOperationResult<T> ToResult<T>(this T entity, HttpStatusCode status = HttpStatusCode.OK, string message = default)
        {
            return new OperationResult<T>(status, message, entity);
        }

        public static IOperationResult ToResult(this Exception ex)
        {
            return new OperationResult(ex);
        }

        public static async Task<IOperationResultList<T>> ToResultListAsync<T>(this Exception ex)
        {
            return await Task.FromResult(new OperationResultList<T>(ex));
        }

        public static async Task<IOperationResult> ToResultAsync(this Exception ex)
        {
            return await Task.FromResult(new OperationResult(ex));
        }

        public static IOperationResultList<T> ToResultList<T>(this Exception ex) where T : class
        {
            return new OperationResultList<T>(ex);
        }

        public static IOperationResult<T> ToResult<T>(this Exception ex) where T : class
        {
            return new OperationResult<T>(ex);
        }

        public static async Task<IOperationResult<T>> ToResultAsync<T>(this Exception ex)
        {
            return await Task.FromResult(new OperationResult<T>(ex));
        }

        public static async Task<IOperationResult<T>> ToRequestAsync<T>(this T entity, HttpStatusCode status = HttpStatusCode.OK, string message = default, string error = default)
        {
            return await Task.FromResult(new OperationResult<T>(status, message, entity, error));
        }

        public static async Task<IOperationResult<T>> ToResultAsync<T>(this T entity, HttpStatusCode status = HttpStatusCode.OK, string message = default, string error = default)
        {
            return await Task.FromResult(new OperationResult<T>(status, message, entity, error));
        }


        public static DateTime? ToDateTime(this string date)
        {
            if (!string.IsNullOrWhiteSpace(date))
            {
                if (DateTime.TryParse(date, CultureInfo.GetCultureInfo("es"), DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }

            return default;
        }
    }
}
