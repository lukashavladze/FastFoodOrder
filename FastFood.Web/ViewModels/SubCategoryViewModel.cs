﻿using FastFood.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Web.ViewModels
{
    public class SubCategoryViewModel : SubCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
    }
}
