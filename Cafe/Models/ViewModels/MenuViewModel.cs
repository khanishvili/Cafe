using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Models.ViewModels
{
    public class MenuViewModel
    {
        public MenuItem MenuItem { get; set; }

        public IEnumerable<Category> Category { get; set; }

        public IEnumerable<SubCategory> SubCategories { get; set; }
        public IEnumerable<Taste> Tastes { get; set; }
    }
}
