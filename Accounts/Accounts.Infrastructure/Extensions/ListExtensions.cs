using Accounts.Domain.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Infrastructure.Extensions
{
    public static class ListExtensions
    {
        public static PaginatedResult<T> Paginate<T>(this IList<T> collection, int pageNumber, int pageSize)
        {
            if (pageNumber < 0 || pageSize < 0)
            {
                throw new ValidationException("Page and size should not be negative values!");
            }

            var totalElements = collection.Count();
            var skip = pageNumber * pageSize;

            if (totalElements == 0 || totalElements < skip)
            {
                return PaginatedResult<T>.EmptyResult(pageNumber);
            }

            var result = collection.Skip(skip).Take(pageSize).ToList();

            return new PaginatedResult<T>(result, pageNumber, pageSize);
        }
    }
}
