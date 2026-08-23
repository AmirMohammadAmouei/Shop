using Microsoft.EntityFrameworkCore;
using Transportation.Buisness._0.Common.Paging;

namespace Transportation.Buisness._0.Common
{
    public static class QueryableExtenstions
    {
        public static async Task<SPFOutPutDto<T>> ToPaginatedListAsync<T>(
       this IQueryable<T> source,
       SPFInputDto paginationRequest,
       CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // دریافت تعداد کل آیتم‌ها
            var totalCount = await source.CountAsync(cancellationToken);

            // اعمال Pagination
            var items = await source
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);

            return new SPFOutPutDto<T>
            {
                Items = items,
                PageNumber = paginationRequest.PageNumber,
                PageSize = paginationRequest.PageSize,
                TotalCount = totalCount
            };
        }

        // اورلود برای زمانیکه نیاز به اعمال sorting یا filtering قبل از pagination دارید
        public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> source,
            SPFInputDto paginationRequest)
        {
            return source
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize);
        }
    }
}
