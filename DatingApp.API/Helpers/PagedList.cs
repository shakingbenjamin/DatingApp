namespace DatingApp.API.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class PagedList<T> : List<T>
    {
        public PagedList(int currentPage, int pageSize, int totalPages, int totalCount)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
            this.TotalCount = totalCount;

        }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int pageNumber, int pageSize, int count)
        {
            // evenly split the items over pages to the nearest int
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalCount = count;
            this.AddRange(items);
        }

        // create new instance of a paged list
        // iQueryable defers the execution action so we can define the query against 
        // the database using skip etc allowing us to page the request
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
        int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            // use skip and take operators to get specific items based off
            // of what is requested
            // EG: page size of 5 and card amount of 15
            // if you want the second page, you skip the first five and take the next
            var items = await source.Skip((pageNumber - 1) * pageSize).
                Take(pageSize).ToListAsync();
            return new PagedList<T>(items, pageNumber, pageSize, count);
        }
    }
}