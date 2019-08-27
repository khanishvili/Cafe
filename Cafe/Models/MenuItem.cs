using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class MenuItem
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Taste Taste { get; set; }
        public string Image { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        [Display(Name = "SubCategory")]
        public int SubCategoryID { get; set; }
        [ForeignKey("SubCategoryID")]
        public virtual SubCategory SubCategory { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Price must to be greate then 1$")]
        public double Price { get; set; }

    }
}
