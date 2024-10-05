using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Account.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;

namespace FoodApp.Api.CQRS.Account.Commands
{
    public record SendVerificationOTP(string Email) : IRequest<Result<bool>>;

    public class ResendVerificationOTPHandler : BaseRequestHandler<SendVerificationOTP, Result<bool>>
    {
        public ResendVerificationOTPHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public async override Task<Result<bool>> Handle(SendVerificationOTP request, CancellationToken cancellationToken)
        {
            var userResult = await _mediator.Send(new GetUserByEmailQuery(request.Email));

            if (!userResult.IsSuccess)
            {
                return Result.Failure<bool>(UserErrors.UserNotFound);
            }
            var user = userResult.Data;
            if(user.IsEmailVerified)
            {
                return Result.Failure<bool>(UserErrors.EmailIsAlreadyVerified);

            }
            var otpCode = GenerateOTP();
            user.VerificationOTP = otpCode;
            user.VerificationOTPExpiration = DateTime.Now.AddMinutes(5);

            var userRepo = _unitOfWork.Repository<User>();
            userRepo.Update(user);
            await userRepo.SaveChangesAsync();

            var emailContent = $"Your OTP code to verify your Account is: {otpCode}";
            await _emailSenderHelper.SendEmailAsync(request.Email, "Verify Your Account", emailContent);

            return Result.Success(true);
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
