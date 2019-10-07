using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels.UserModels
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(vm => vm.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(6).WithMessage("Password must contains more than 6 characters");
            RuleFor(vm => vm.Username)
                .NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(vm => vm.FirstName)
                .NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(vm => vm.LastName)
                .NotEmpty().WithMessage("Username cannot be empty");
        }
    }
}
