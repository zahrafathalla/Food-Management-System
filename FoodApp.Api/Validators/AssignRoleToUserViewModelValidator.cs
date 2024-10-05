using FluentValidation;
using FoodApp.Api.ViewModels;

namespace FoodApp.Api.Validators
{
    public class AssignRoleToUserViewModelValidator : AbstractValidator<AssignRoleToUserViewModel>
    {
        public AssignRoleToUserViewModelValidator()
        {
            RuleFor(x => x.RoleName)
                 .NotEmpty().WithMessage("RoleName is required");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId Is required");
        }
    }
}
