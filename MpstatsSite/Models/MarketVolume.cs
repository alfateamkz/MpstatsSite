using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MpstatsSite.Models
{
    public class MarketVolume
    {
        public DateTime Date { get; set; }
        public double OurVolume { get; set; }
        public double TotalVolume { get; set; }
        public double OurPercent { get; set; }
    }
}