using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheRichLifeProject.ViewModel
{
    public class EditProduct : Controller
    {
        public string ImageCaption { get; set; }
        public string ImageDescription { get; set; }
        public IFormFile Image { get; set; }
    }
        
}
