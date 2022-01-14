using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBS_Manteniemientos_Financial.Cliente.Helpers
{
    public sealed class AsociadoEsVerdadero : ValidationAttribute
    {
        private readonly string NombrePropiedadValidar;

        public AsociadoEsVerdadero(string nombrePropiedadValidar)
        {
            this.NombrePropiedadValidar = nombrePropiedadValidar;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.NombrePropiedadValidar);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("{0} no es una propiedad existente", this.NombrePropiedadValidar, new List<string> { validationContext.MemberName }));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            if (!(propertyTestedValue is bool))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new List<string> { validationContext.MemberName });
            }
            else if ((bool)propertyTestedValue && value != null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new List<string> { validationContext.MemberName });
        }
    }
}