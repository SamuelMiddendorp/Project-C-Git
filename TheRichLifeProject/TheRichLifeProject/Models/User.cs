using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }       
        public DateTime Birth { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } 
        public DateTime DateRegistered { get; set; }
        public string Adress { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Order> Orders { get; set; }
    }
}
