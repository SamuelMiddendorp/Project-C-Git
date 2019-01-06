using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public class Review
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public DateTime PublishDate { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public User User { get; set; }
    }
}
