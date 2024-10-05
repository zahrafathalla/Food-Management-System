using FluentValidation;
using FoodApp.Api.ViewModels;

namespace FoodApp.Api.Validators
{
    public class CreateRoleViewModelValidator: AbstractValidator<CreateRoleViewModel>
    {
        public CreateRoleViewModelValidator()
        {
            RuleFor(x => x.RoleName)
                 .NotEmpty().WithMessage("RoleName is required");
        }
    }
}
