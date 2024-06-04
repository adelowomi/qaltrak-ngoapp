using System.ComponentModel.DataAnnotations;

namespace NGOAPP;

/// <summary>
/// this class is used to validate the sort input, input must be either asc or desc
/// </summary>
public class SortInputValidator : ValidationAttribute
{
    /// <summary>
    /// this method is used to validate the sort input
    /// </summary>
    /// <param name="value">the value to be validated</param>
    /// <param name="validationContext">the validation context</param>
    /// <returns>returns a validation result</returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var sort = value.ToString().ToLower();
        if (sort != "asc" && sort != "desc")
            return new ValidationResult("Sort must be either 'asc' or 'desc'");

        return ValidationResult.Success;
    }

}
