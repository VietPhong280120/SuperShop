using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Systems.User
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required")
                .MaximumLength(200).WithMessage("First Name can not over 200 characters");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required")
               .MaximumLength(200).WithMessage("Last Name can not over 200 characters");

            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Birthday can not greather than 100 year");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required ")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email format not match");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User Name is required");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password is at 6 characters");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Confirm password is not match");
                }
            });
        }
    }
}