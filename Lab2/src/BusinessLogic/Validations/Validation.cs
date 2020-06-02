using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eventus.BusinessLogic.Validations
{
    public static class Validation
    {
        public static IEnumerable<ValidationResult> IsValid<T>(this T model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }
    }
}