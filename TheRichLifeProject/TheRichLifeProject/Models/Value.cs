using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public class Value
    {
        public int ValueId { get; set; }
        public string ValueValue { get; set; }
        public Attribute Attribute { get; set; }
        public Product Product { get; set; }
    }
}
