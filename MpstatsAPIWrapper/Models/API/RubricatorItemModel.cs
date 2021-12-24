using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsAPIWrapper.Models.API
{
    public class RubricatorItemModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<SubcategoryModel> Subcategories { get; set; }
    }
}
