using API.Dto;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace API.Utilities
{
    public static class IQueryableExtensions 
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDto paginacionDto)
        {
            return  queryable
                    .Skip((paginacionDto.Pagina -1) * paginacionDto.RecordsPorPagina)
                    .Take(paginacionDto.RecordsPorPagina);
        }


    }
}
