using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public class Atribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Value> Values { get; set; }
    }
}
