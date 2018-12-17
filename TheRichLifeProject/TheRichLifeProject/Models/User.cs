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

        [Required, StringLength(16, MinimumLength = 2)]
        public string Username { get; set; }

        [Required, StringLength(16, MinimumLength = 2)]
        public string Name { get; set; }

        [Required, StringLength(16, MinimumLength = 2)]
        [Display(Name="Sur Name")]
        public string SurName { get; set; }

        [Required, DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        public string Password { get; set; }

        [Display(Name="Phone Number")]
        //Regular expression for checking dutch phone numbers
        //[RegularExpression(@"^\+[0 - 9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0 - 9]{9}$|[0-9\-\s]{10}$")]
        //[RegularExpression("^[0 - 9] *$")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }   
        
        [Required, Display(Name = "Date of Birth")]
        [MinimumAge(18)]
        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }

        [Required, Display(Name="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public Role Role { get; set; } 

        [Display(Name="Date Registered")]
        public DateTime DateRegistered { get; set; }

        [Required]
        public Province Province { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string City { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 6)]
        //[RegularExpression("/^[1 - 9][0 - 9]{3} ? (?!sa|sd|ss)[a-z]{2}$/i")]
        public string Zip { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 7)]
        public string Address { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Order> Orders { get; set; }
    }
}
