using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public static class DataExtensions
    {
        public static EntityEntry SetProperty(this EntityEntry entry, string propertyName, object value)
        {
            try
            {
                if (entry != null && entry.Properties.Any(x => x.Metadata.Name == propertyName))
                {
                    var prop = entry.Property(propertyName);

                    if (prop != null) prop.CurrentValue = value;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return entry;
        }

        public static T SetProperty<T>(this DbSet<T> dbSet, T entity, string name, object value) where T : class
        {
            var service = dbSet.GetService<ICurrentDbContext>();

            if (service != null)
            {
                var context = service.Context;

                if (context != null)
                {
                    context.Entry(entity)
                           .SetProperty(name, value);
                }
            }

            return (entity);
        }
    }


}
