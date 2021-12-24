using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpstatsAPIWrapper.Models.API
{
    public class BrandSeller
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Seller { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public int? Balance { get; set; }
        public int? Comments { get; set; }
        public int? Rating { get; set; }
        public double? FinalPrice { get; set; }
        public int? Sales { get; set; }
        public double? Revenue { get; set; }
        public int? DaysInStock { get; set; }
    }
}
