using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validations
{
    public static class CustomValidator
    {
        public static IRuleBuilderOptions<T, IFormFile> CheckFileFormat<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, string fileFormat)
        {
            return ruleBuilder.Must(x => fileFormat.Contains(Path.GetExtension(x.FileName).ToLower()));
        }

        public static bool Validate(this Guid value)
        {
            return value != Guid.Empty;
        }
        public static bool Validate(this Guid? value)
        {
            return value != null && value != Guid.Empty && Guid.TryParse(value.ToString(), out Guid result);
        }
    }
}
