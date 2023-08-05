using Application.DTOs;
using FluentValidation;

namespace Application.Validations;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required")
                     .EmailAddress().WithMessage("A valid email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be null or empty");
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token cannot be null or empty");
    }
}
