using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MpstatsSite.Models;

namespace MpstatsSite.Helpers
{
    public static class MpstatsParsedHelper
    {
        public static List<MarketCategoryInfo> CategoryInfos { get; set; } = new List<MarketCategoryInfo>();

        public static MarketCategoryInfo GetCategoryInfo(string name)
        {
            var info = CategoryInfos.FirstOrDefault(o => o.Category.Contains(name)
                                                         || o.Category.Contains(name.ToLower()) ||
                                                         o.Category.Contains(name.ToUpper()));
            if (info != null)
            {
                return info;
            }
            else
            {
                return new MarketCategoryInfo();
            }
        }

    }
}