using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Account.Commands;
using FoodApp.Api.CQRS.Account.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.CQRS.Account.Orchestrator
{
    public record RegisterOrchestrator(
   string UserName,
   string Email,
   string Country,
   string PhoneNumber,
   string Password,
   string ConfirmPassword) : IRequest<Result<bool>>;

    public class RegisterOrchestratorHandler : BaseRequestHandler<RegisterOrchestrator, Result<bool>>
    {
        public RegisterOrchestratorHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public async override Task<Result<bool>> Handle(RegisterOrchestrator request, CancellationToken cancellationToken)
        {
            var command = request.Map<RegisterCommand>();

            var RegisterResult = await _mediator.Send(command);

            if (!RegisterResult.IsSuccess)
            {
                return Result.Failure<bool>(UserErrors.UserDoesntCreated);
            }

           await _mediator.Send(new SendVerificationOTP(request.Email));

            return Result.Success(true);
        }
    }
}
