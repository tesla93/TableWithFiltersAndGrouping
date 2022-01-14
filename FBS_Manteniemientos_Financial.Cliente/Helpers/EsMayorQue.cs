using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBS_Manteniemientos_Financial.Cliente.Helpers
{
    public sealed class EsMayorQue : ValidationAttribute
    {
        private readonly string NombrePropiedadValidar;
        private readonly bool PermiteValoresIguales;

        public EsMayorQue(string nombrePropiedadValidar, bool permiteValoresIguales = false)
        {
            this.NombrePropiedadValidar = nombrePropiedadValidar;
            this.PermiteValoresIguales = permiteValoresIguales;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.NombrePropiedadValidar);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("unknown property {0}", this.NombrePropiedadValidar, new List<string> { validationContext.MemberName }));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);
            if (!(value is int))
            {
                if (value is decimal)
                {
                    if (!(propertyTestedValue is decimal))
                    {
                        return ValidationResult.Success;
                    }
                    var value1 = (decimal)value;
                    var value2 = (decimal)propertyTestedValue;
                    var flag = this.PermiteValoresIguales;

                    if (value1 >= value2)
                    {
                        if (flag && value1 == value2)
                        {
                            return ValidationResult.Success;
                        }
                        else if (value1 > value2)
                        {
                            return ValidationResult.Success;
                        }
                    }
                }
            }
            if (!(value is decimal))
            {
                if (value is int)
                {
                    if (!(propertyTestedValue is int))
                    {
                        return ValidationResult.Success;
                    }
                    var value1 = (int)value;
                    var value2 = (int)propertyTestedValue;
                    var flag = this.PermiteValoresIguales;
                    if (value1 >= value2)
                    {
                        if (flag && value1 == value2)
                        {
                            return ValidationResult.Success;
                        }
                        else if (value1 > value2)
                        {
                            return ValidationResult.Success;
                        }
                    }
                }
            }

            //if (!(value is decimal) && !(value is int))
            //{
            //    if (value is DateTime)
            //    {
            //        if (!(propertyTestedValue is DateTime))
            //        {
            //            return ValidationResult.Success;
            //        }
            //        if ((DateTime)value >= (DateTime)propertyTestedValue)
            //        {
            //            if (this.PermiteValoresIguales && value == propertyTestedValue)
            //            {
            //                return ValidationResult.Success;
            //            }
            //            else if ((DateTime)value > (DateTime)propertyTestedValue)
            //            {
            //                return ValidationResult.Success;
            //            }
            //        }
            //    }
            //}

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new List<string> { validationContext.MemberName });
        }
    }
}