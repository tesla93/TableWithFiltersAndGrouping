using System;
using System.ComponentModel.DataAnnotations;

namespace FBS_Manteniemientos_Financial.Cliente.Helpers
{
    public sealed class FechaAntesDe : ValidationAttribute
    {
        private readonly string NombrePropiedadValidar;
        private readonly bool PermiteValoresIguales;

        public FechaAntesDe(string NombrePropiedadValidar, bool PermiteValoresIguales = false)
        {
            this.NombrePropiedadValidar = NombrePropiedadValidar;
            this.PermiteValoresIguales = PermiteValoresIguales;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.NombrePropiedadValidar);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("{0} no es un DataTime", this.NombrePropiedadValidar));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            if (value == null || !(value is DateTime))
            {
                return ValidationResult.Success;
            }

            if (propertyTestedValue == null || !(propertyTestedValue is DateTime))
            {
                return ValidationResult.Success;
            }

            // Compare values
            if ((DateTime)value >= (DateTime)propertyTestedValue)
            {
                if (this.PermiteValoresIguales && value == propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
                else if ((DateTime)value > (DateTime)propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}