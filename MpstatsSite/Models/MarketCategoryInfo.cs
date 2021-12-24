using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MpstatsSite.Models
{
    public class MarketCategoryInfo
    {
        public List<MarketVolume> VolumeForLastMonth { get; set; } = new List<MarketVolume>();
        public string Category { get; set; } = String.Empty;

        public double OurPercent { get; set; }
    }
}