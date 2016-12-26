using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.Web.Models
{
    public class PagedList<T> : List<T>
    {
        public static PagedList<T> Empty => Create(Enumerable.Empty<T>(), 0, 0, 0);

        private PagedList(List<T> items, int count, int pageIndex, int pageSize, int numberOfButtons)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count/(double) pageSize);
            Stats = count;
            // avoid to set number of buttons more then pages total.
            NumberOfPagerButtons = Math.Min(numberOfButtons, TotalPages);

            AddRange(items);
        }

        [Display(Name = "Search")]
        public string CurrentFilter { get; set; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int Stats { get; }
        public int NumberOfPagerButtons { get; }

        public bool SelectAll { get; set; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public bool IsDotsNeeded => NumberOfPagerButtons < TotalPages;

        public static PagedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize, int numberOfButtons)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1)*pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageIndex, pageSize, numberOfButtons);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize,
            int numberOfButtons)
        {
            var count = await source.CountAsync();
            if (pageIndex > 1)
                source = source.Skip((pageIndex - 1)*pageSize);
            var items = await source.Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageIndex, pageSize, numberOfButtons);
        }
    }
}