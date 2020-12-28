using System;
using Microsoft.AspNetCore.Http;

namespace BookwormsAPI.Extensions
{
    public static class HttpContextExtensions
    {
        public static void AddPaginationResponseHeaders(
                this HttpContext httpContext,
                int recordCount,
                int pageSize,
                int pageIndex
            )
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            httpContext.Response.Headers.Add("Pagination-Count", recordCount.ToString());
            httpContext.Response.Headers.Add("Pagination-Page", pageIndex.ToString());
            httpContext.Response.Headers.Add("Pagination-Limit", pageSize.ToString());
        }
    }
}