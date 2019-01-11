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

        [Required(ErrorMessage = "Username is required"), StringLength(16, MinimumLength = 2, 
            ErrorMessage = "Username must be at least 2 characters and at max 16 characters long")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Name is required"), StringLength(16, MinimumLength = 2, 
            ErrorMessage = "Name must be at least 2 characters and at max 16 characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Sur Name is required"), StringLength(16, MinimumLength = 2,
            ErrorMessage = "Surname must at least 2 characters and at max 16 characters long")]
        [Display(Name="Sur Name")]
        public string SurName { get; set; }

        [Required, DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$",
            ErrorMessage = "Please make sure your password contains at least: 1 capital and non-captital letter, " +
            "a digit, a special character and needs to be a 8 characters long at least")]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber), Display(Name="Phone Number"), StringLength(10, MinimumLength = 10)]
        [RegularExpression("06\\d{8}", ErrorMessage = "Please make sure to enter a valid phonenumber starting with 06")]
        public string PhoneNumber { get; set; }   
        
        [Required, Display(Name = "Date of Birth")]
        [MinimumAge(18)]
        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }

        [Required, Display(Name="Email Address"), DataType(DataType.EmailAddress)]
        [RegularExpression("[\\w_.+-]+@[\\w-]+\\.[\\w.]+", ErrorMessage = "Please enter a valid Email Address")]
        public string Email { get; set; }

        [Required]
        public Role Role { get; set; } 

        [Display(Name="Date Registered")]
        public DateTime DateRegistered { get; set; }

        [Required]
        public Province Province { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(20, MinimumLength = 3)]
        public string City { get; set; }

        [Required(ErrorMessage = "Zip is required")]
        [StringLength(7, MinimumLength = 6, ErrorMessage = "Please enter a valid zip")]
        [RegularExpression("\\d{4}\\s?[a-zA-Z]{2}", ErrorMessage = "Please enter a valid zip")]
        public string Zip { get; set; }

        [Required, Display(Name = "Street and house number")]
        [StringLength(32, MinimumLength = 7)]
        [RegularExpression("[\\w-]+\\s+\\d+", ErrorMessage = "Please enter a valid street name and house number")]
        public string Address { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Order> Orders { get; set; }
    }
}
