using System;
using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Components;

namespace BookwormsUI.Extensions
{
    public static class NavigationManagerExtensions
    {
        // get the entire querystring as a NameValueCollection
        public static NameValueCollection GetQueryStringCollection(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }

        // get a specific querystring value from its key
        public static string GetQueryString(this NavigationManager navigationManager, string key)
        {
            return navigationManager.GetQueryStringCollection()[key];
        }
    }
}