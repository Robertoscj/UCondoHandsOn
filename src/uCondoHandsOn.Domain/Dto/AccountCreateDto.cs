using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using uCondoHandsOn.Domain.Validation;
using ValidationResult = uCondoHandsOn.Domain.Validation.ValidationResult;

namespace uCondoHandsOn.Domain.Dto
{
    public class AccountCreateDto : AccountDto, IValidationDto
    {
        public string ? ParentCode { get; set; }
        public ValidationResult IsValid()
        {
           var validations = new List<ValidationResult>
    {
        string.IsNullOrWhiteSpace(Name)
            ? ValidationResult.Fail("Campo nome não pode ser vazio.")
            : ValidationResult.Success(),

        string.IsNullOrWhiteSpace(Code)
            ? ValidationResult.Fail("Campo do código não pode ser vazio.")
            : ValidationResult.Success(),

        CheckCodeFormat() 
    };

    return validations.FirstOrDefault(v => v.Invalid) ?? ValidationResult.Success();
        }

         private ValidationResult CheckCodeFormat()
        {
            var codeRegex = new Regex(@"^[0-9][0-9.]*$");
            
            if (!codeRegex.IsMatch(Code))
                return ValidationResult.Fail("formato do código está inválido. Um código válido é um número, seguido de ponto (.) e então seguido de outro número, com vários níveis.");

            var slices = Code.Split(".");

            foreach (var slice in slices)
            {
                _ = long.TryParse(slice, out var longSlice);

                if (longSlice < 1 || longSlice > 999)
                    return ValidationResult.Fail("formato do código está inválido. Cada nível do código da conta precisa estar entre 1 e 999 (inclusive).");
            }

            return ValidationResult.Success();
        }
    }
}