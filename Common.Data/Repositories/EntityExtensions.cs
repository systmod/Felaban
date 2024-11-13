
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class EntityExtensions
    {
        public static IQueryable<T> Search<T>(this DbSet<T> entity, Expression<Func<T, bool>> predicate) where T : class
        {
            return entity.Where(predicate);
        }

        public static T GetByID<T>(this DbSet<T> entity, params object[] keys) where T : class
        {
            return ((T)entity.Find(typeof(T), keys));
        }

        public static T Insert<T>(this DbSet<T> entity, T value) where T : class
        {
            return (entity.Add(value)).Entity;
        }

        public static async Task<T> InsertAsync<T>(this DbSet<T> entity, T value) where T : class
        {
            return (await entity.AddAsync(value)).Entity;
        }

        public static async Task<T> RemoveAsync<T>(this DbSet<T> entity, T value) where T : class
        {
            return await Task.FromResult(entity.Remove(value).Entity);
        }

        public static async Task<T> DeleteAsync<T>(this DbSet<T> entity, T value) where T : class
        {
            entity.SetProperty(value, "IdEstado", 0);
            return await UpdateAsync(entity, value);
        }

        public static async Task<T> UpdateAsync<T>(this DbSet<T> context, T value) where T : class
        {
            return await Task.FromResult(context.Update(value).Entity);
        }

        public static async Task<ICollection<T>> InsertAllAsync<T>(this DbSet<T> entity, ICollection<T> list) where T : class
        {
            foreach (var item in list)
            {
                await entity.InsertAsync(item);
            }

            return await Task.FromResult(list);
        }

        public static async Task<ICollection<T>> UpdateAllAsync<T>(this DbSet<T> entity, ICollection<T> list) where T : class
        {
            foreach (var item in list)
            {
                await entity.UpdateAsync(item);
            }

            return await Task.FromResult(list);
        }

        public static async Task<int> SaveAsync(this DbContext context, IOperationRequest request)
        {
            return await SaveAsync(context, request.Empresa.IdEmpresa, request.InicioSesion, request.Ip);
        }

        public static async Task<int> SaveAsync(this DbContext context, int idEmpresa = default, string usuario = default, string ip = default)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified)
                {

                    entry.SetProperty("UsuarioModificacion", usuario ?? "DESCONOCIDO")
                        .SetProperty("FechaModificacion", DateTime.Now)
                        .SetProperty("IpModificacion", ip ?? "0.0.0.0")
                        .SetProperty("IdEmpresa", idEmpresa);

                }

                if (entry.State == EntityState.Added)
                {
                    entry.SetProperty("UsuarioRegistro", usuario ?? "DESCONOCIDO")
                        .SetProperty("FechaRegistro", DateTime.Now)
                        .SetProperty("IpRegistro", ip ?? "0.0.0.0")
                        .SetProperty("IdEmpresa", idEmpresa)
                        .SetProperty("IdEstado", true);
                }

            }

            return await context.SaveChangesAsync();
        }

    }
}
