using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Systems.User
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User Name is require");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is require")
                .MinimumLength(6).WithMessage("Password is at 6 characters");
        }
    }
}