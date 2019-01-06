﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.ViewModel
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
