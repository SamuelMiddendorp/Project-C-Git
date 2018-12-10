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

        [Required]
        [StringLength(16, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 3)]
        [Display(Name="Sur Name")]
        public string SurName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        public string Password { get; set; }

        [Display(Name="Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }   
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }

        [Required]
        [Display(Name="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public Role Role { get; set; } 

        [Display(Name="Date Registered")]
        public DateTime DateRegistered { get; set; }

        [Required]
        public string Address { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Order> Orders { get; set; }
    }
}
