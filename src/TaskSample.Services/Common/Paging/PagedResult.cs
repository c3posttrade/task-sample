using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskSample.Services.Common.Paging
{
    public class PagedResult<T> where T : class
    {
        public int Page { get; private set; }
        public int PageSize { get; private set; } = 10;
        public int TotalPages { get; private set; }
        public int TotalRecords { get; private set; }
        public IEnumerable<T> Data { get; private set; }
        public PagedResult(IEnumerable<T> source, int totalRecords, int pageNumber, int pageSize)
        {
            TotalRecords = totalRecords;
            Page = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            Data = source.Take(pageSize).ToList();
        }

        public PagedResult(IEnumerable<T> source, int totalRecords, PagingModel paging)
        {
            TotalRecords = totalRecords;
            Page = paging.PageNumber;
            PageSize = paging.PageSize;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)paging.PageSize);
            Data = source.Take(paging.PageSize).ToList();
        }

    }
}
