using System.Linq.Expressions;
using DripChip.Models.Entities;
using Microsoft.OpenApi.Extensions;

namespace DripChip;

public static class Extensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, bool>> whereClause)
    {
        return condition ? query.Where(whereClause) : query;
    }
}