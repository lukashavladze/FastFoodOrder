using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Models
{
    public class Item
    {
        public int Id { get; set; } 

        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public int CategoryId { get; set; } 

        public Category Category { get; set; }

        [ForeignKey(nameof(SubCategoryId))]
        public int SubCategoryId { get; set; }  

        public SubCategory SubCategory { get; set; }
    }
}
