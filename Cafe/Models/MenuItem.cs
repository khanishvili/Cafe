﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class MenuItem
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Descritptions { get; set; }

        public string SpiCyness { get; set; }

        public enum ESpicy { NA = 0, NotSpicy = 1, Spicy = 2, VerySpicy = 3 }

        public string Image { get; set; }

        [Display(Name="Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }
        [ForeignKey("ID")]
        public virtual SubCategory SubCategory { get; set; }


        [Range(1,int.MaxValue,ErrorMessage ="Price Should Be greated than $1")]
        public double Price { get; set; }



    }

}
