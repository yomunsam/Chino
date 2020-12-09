using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Microsoft.EntityFrameworkCore;

namespace Chino.Utils.PaginatedList
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public int ItemCount { get; set; }

        public PaginatedList(IList<T> items, int count, int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);

            this.ItemCount = count;
        }

        /// <summary>
        /// 创建分页数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public PaginatedListDto<T> GetDto()
        {
            return new PaginatedListDto<T>
            {
                Data = this,
                PageIndex = this.PageIndex,
                TotalPages = this.TotalPages,
                ItemCount = this.ItemCount
            };
        }
    }
}
