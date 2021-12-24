using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MpstatsSite.Models;

namespace MpstatsSite.ViewModels
{
    public class IndexViewModel
    {
        public MarketCategoryInfo Info { get; set; }
        public string Category { get; set; }
        public double OurTotalVolume { get; set; }
    } 
}