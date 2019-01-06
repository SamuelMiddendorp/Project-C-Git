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

        //TODO: Add Regex
        [Required, DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        [RegularExpression("^(?=.*[a-z])")]
        public string Password { get; set; }

        //TODO: Add Regex
        [Display(Name="Phone Number")]
        [StringLength(11, MinimumLength = 10)]
        //Regular expression for checking dutch phone numbers
        //[RegularExpression(@"^\+[0 - 9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0 - 9]{9}$|[0-9\-\s]{10}$")]
        //[RegularExpression("^[0 - 9] *$")]
        //[RegularExpression("(+?\\d{10,11})")]
        
        // 0651712452
        // 07021456385 0705423541
        // +31654247216 
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }   
        
        [Required, Display(Name = "Date of Birth")]
        [MinimumAge(18)]
        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }

        //TODO: Add Regex
        [Required, Display(Name="Email Address")]
        [RegularExpression("[\\w_.+-]+@[\\w-]+\\.[\\w.]+")]
        [DataType(DataType.EmailAddress)]
        // test@test.com
        // test#test.com
        // test34@gmail.nl
        // test@hotmil.com.nl
        // test.test@gmail.com.com

        //TODO: Add Regex
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
        [RegularExpression("\\d{4}\\s?[a-zA-Z]{2}")]
        // 2254 JE
        // 4523BD
        // 4523 JD
        // 6241BD
        // 45123ND
        public string Zip { get; set; }

        [Required]
        [Display(Name = "Address and address number")]
        [StringLength(32, MinimumLength = 7)]
        [RegularExpression("[\\w-]+")]
        // jfiefoew 33
        // jdlsdad33
        // jsdiowwo 22
        // st-stuart laan 34
        public string Address { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Order> Orders { get; set; }
    }
}
