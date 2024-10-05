using FluentValidation;
using FoodApp.Api.ViewModels;

namespace FoodApp.Api.Validators
{
    public class ResetPasswordViewModelValidator :AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.OTP)
                .NotEmpty().WithMessage("OTP is required");

            RuleFor(x => x.NewPassword)
                 .NotEmpty().WithMessage("Password is required");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required");
        }
    }
}
