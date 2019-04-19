using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult("Date required");
            }
            DateTime dateTimeValue = (DateTime) value;
            int result = DateTime.Compare(dateTimeValue, DateTime.Now);
            // if result is less than 0, the value is before now
            if (result > 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date must be in the future");
            }
        }
    }
}