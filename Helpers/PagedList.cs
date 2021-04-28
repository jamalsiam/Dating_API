using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api.Helpers
{
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }       
        public int TotalCount { get; set; }
        public IEnumerable<T> List { get; set; }

         public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize){
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber -1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(){
                List = items,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalCount = count
            };
        }
    }
}