using CnxTaskManagement.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Common
{
    public class SearchResult<T> : IPagedResult
    {
        public List<T> data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public string Keyword { get; set; } = string.Empty;

        public SearchResult()
        {
            data = new List<T>();
        }
    }
}
