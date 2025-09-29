using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Commons
{
    public class SuccessResponse
    {
        public bool Success { get; }
        public dynamic? Data { get; set; }

        public SuccessResponse()
        {
            Success = true;
        }

        public SuccessResponse(dynamic data)
        {
            Success = true;
            Data = data;
        }
    }

    public class SuccessResponse<T>
    {
        public SuccessResponse()
        {
            Success = true;
        }
        public SuccessResponse(T data)
        {
            Success = true;
            Data = data;
        }

        public bool Success { get; }
        public T Data { get; set; } = default!;
    }

    public class SuccessPagingResponse<T>
    {
        public SuccessPagingResponse()
        {
            Success = true;
        }
        public bool Success { get; }
        public PagingResponse<T> Data { get; set; } = new PagingResponse<T>();
    }

    public class SuccessPagingResponse<T, TSummary>
    {
        public SuccessPagingResponse()
        {
            Success = true;
        }
        public bool Success { get; }
        public PagingResponse<T, TSummary> Data { get; set; } = new PagingResponse<T, TSummary>();
    }

    public class PagingResponse<T>
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
        public List<T>? Data { get; set; }
    }
    public class PagingResponse<T, TSummary>
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
        public List<T>? Data { get; set; }
        public TSummary? Summary { get; set; }
    }
}
