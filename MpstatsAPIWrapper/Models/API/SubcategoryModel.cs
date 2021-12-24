﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsAPIWrapper.Models.API
{
    public class SubcategoryModel : RubricatorItemModel
    {
        new public List<SubcategoryModel> Subcategories { get; set; }
        new public string Name { get; set; }
        public int? ItemsNumber { get; set; }
        public int? SalesNumber { get; set; }
        public double? Revenue { get; set; }
        public double? CommentsAverage { get; set; }
        public double? Rating { get; set; }

        public override string ToString()
        {
            return $"{Name}  Продажи на {Revenue} рублей";
        }
    }
}
