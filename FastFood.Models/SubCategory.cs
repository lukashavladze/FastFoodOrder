﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public string Title { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        
        public ICollection<Item> Items { get; set; } 
    }
}
