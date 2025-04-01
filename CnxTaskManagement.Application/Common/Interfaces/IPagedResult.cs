using System.Collections.Generic;

namespace CnxTaskManagement.Application.Common.Interfaces
{
    public interface IPagedResult
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalPages { get; set; }
        int TotalRecords { get; set; }
        string Keyword { get; set; }
    }
}
