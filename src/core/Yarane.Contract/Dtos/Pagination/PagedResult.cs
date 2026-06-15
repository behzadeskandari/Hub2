using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Contract.Dtos.Pagination
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }

    public class PagedRequest
    {
        public int Page { get; set; } 
        public int PageSize { get; set; } 
    }
}
