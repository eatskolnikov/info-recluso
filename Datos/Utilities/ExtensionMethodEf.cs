using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Datos.Utilities;

public static class ExtensionMethodEf
{
    public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes)
    where T : class
    {
        if (includes != null)
        {
            query = includes.Aggregate(query,
                      (current, include) => current.Include(include));
        }

        return query;
    }
}