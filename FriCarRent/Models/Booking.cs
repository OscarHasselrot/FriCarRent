using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FriCarRent.Models
    
{
    public class Booking
    {
        public int Id { get; set; }
        [ValidateNever]
        public Car Car { get; set; }
        [ValidateNever]
        public Customer Customer { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DateGreaterThan(nameof(StartDate), ErrorMessage = "Slutdatum måste vara efter startdatum.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public float TotalPrice { get; set; }


    }
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var comparisonValue = (DateTime)validationContext.ObjectType
                .GetProperty(_comparisonProperty)
                .GetValue(validationContext.ObjectInstance);

            if (value is DateTime currentValue && currentValue < comparisonValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
