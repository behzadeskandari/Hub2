using FluentValidation;
using NehzatMelli.Dtos;
using NehzatMelli.Messages;


namespace NehzatMelli.Validations
{
    public class NationalCodeRequestValidator : AbstractValidator<NationalCodeRequest>
    {
        public NationalCodeRequestValidator()
        {
            RuleFor(request => request.NationalCode.ToString())
               .NotEmpty()
               .WithMessage(GeneralMessages.NationalIdIsRequiredMessage)
               .Must(dateString => !dateString.Any(char.IsLetter))
               .WithMessage(GeneralMessages.NationalIdMustBeNumbeMessage)
               .Matches(@"^\d{10}$")
               .WithMessage(GeneralMessages.NationalIdPatternMessage);
        }
    }
}
