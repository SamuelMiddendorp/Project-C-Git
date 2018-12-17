using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TheRichLifeProject.Models
{
    public class MinimumAge : ValidationAttribute
    {
        int _minimumAge;

        public MinimumAge(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            User user = (User)validationContext.ObjectInstance;

            if (user.Name == "test" && user.Birth.Year > _minimumAge )
            {
                return new ValidationResult("Minimum Age must be 18");
            }

            return ValidationResult.Success;
        }
    }
}