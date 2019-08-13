using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Models.ViewModels
{
    public class SubCategoryandCategoryModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<string> SubCategoryListName { get; set; }
        public string StatusMessage { get; set; }
    }
}
