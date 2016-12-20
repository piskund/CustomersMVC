using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.Web
{
    public class PagedList<T> : List<T>
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int Stats { get; }
        public bool SelectAll { get; set; }

        private PagedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Stats = count;

            this.AddRange(items);
        }

        public bool HasPreviousPage => (PageIndex > 1);

        public bool HasNextPage => (PageIndex < TotalPages);

        public static PagedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            if (pageIndex > 1)
            {
                source = source.Skip((pageIndex - 1)*pageSize);
            }
            var items = await source.Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}