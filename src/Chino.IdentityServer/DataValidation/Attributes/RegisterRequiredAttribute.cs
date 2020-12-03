using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Resources.DataAnnotation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Nekonya;

namespace Chino.IdentityServer.DataValidation.Attributes
{
    public class RegisterRequiredAttribute : ValidationAttribute
    {
        public RegisterRequiredType RegisterRequiredType { get; set; }

        public RegisterRequiredAttribute(RegisterRequiredType type = RegisterRequiredType.UserName) { this.RegisterRequiredType = type; }

        protected override ValidationResult IsValid(object _value, ValidationContext validationContext)
        {
            string value = (string)_value;
            var configuration = validationContext.GetService<ChinoAccountConfiguration>();
            var factory = validationContext.GetService<IStringLocalizerFactory>();

            var assemblyName = new AssemblyName(typeof(DataAnnotationResources).GetTypeInfo().Assembly.FullName);
            var localizer = factory.Create($"DataAnnotation.{nameof(DataAnnotationResources)}", assemblyName.Name);

            switch (this.RegisterRequiredType)
            {
                case RegisterRequiredType.UserName:
                    if (configuration.UserName.RegisterRequire && value.IsNullOrEmpty())
                        return new ValidationResult(localizer["username_required"]);
                    break;
                case RegisterRequiredType.Email:
                    if(configuration.Email.RegisterRequire && value.IsNullOrEmpty())
                        return new ValidationResult(localizer["email_required"]);
                    break;
                case RegisterRequiredType.PhoneNumber:
                    if (configuration.Phone.RegisterRequire && value.IsNullOrEmpty())
                        return new ValidationResult(localizer["phone_number_required"]);
                    break;
            }
            
            return ValidationResult.Success;
        }
    }
}
