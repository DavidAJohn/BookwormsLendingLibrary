using System;
using System.Collections.Generic;
using System.Linq;

namespace BookwormsUI.Extensions
{
    public static class StringArrayExtensions
    {
        /// <summary>
        /// Returns boolean value depending on the existence of items in an array
        /// </summary>
        public static bool ContainsInArray(this string[] arr, string[] containsArr)
        {
            if (containsArr.Length == 0) return false;

            int containsCount = 0;

            foreach (string el in containsArr)
            {
                if (arr.Contains(el)) 
                {
                    containsCount++;
                }
            }

            return containsCount > 0 ? true : false;
        }
        
    }
}