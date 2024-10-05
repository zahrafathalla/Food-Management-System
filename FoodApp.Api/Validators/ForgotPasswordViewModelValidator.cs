using FluentValidation;
using FoodApp.Api.ViewModels;

namespace FoodApp.Api.Validators
{
    public class ForgotPasswordViewModelValidator :AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator()
        {

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required");
        }

    }
}
