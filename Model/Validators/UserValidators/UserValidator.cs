using FluentValidation;
using Model.Entities;
using Model.ViewModels.UserModels;

namespace Model.Validators.UserValidators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(vm => vm.Username)
                .NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(vm => vm.FirstName)
                .NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(vm => vm.LastName)
                .NotEmpty().WithMessage("Username cannot be empty");
        }
    }
}
