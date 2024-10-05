using FluentValidation;
using FoodApp.Api.ViewModels;

namespace FoodApp.Api.Validators
{
    public class VerifyViewModelValidator :AbstractValidator<VerifyViewModel>
    {
        public VerifyViewModelValidator()
        {
            RuleFor(x=>x.Email).NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.OTP).NotEmpty().WithMessage("OTP is required");
        }
    }
}
