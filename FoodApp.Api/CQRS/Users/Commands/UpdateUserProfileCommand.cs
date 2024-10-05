using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Account.Commands;
using FoodApp.Api.CQRS.Account.Queries;
using FoodApp.Api.CQRS.Users.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.CQRS.Users.Commands
{
    public record UpdateUserProfileCommand
    (string? UserName,
     string? Email,
     string? Country,
     string? PhoneNumber) : IRequest<Result<bool>>;

    public class UpdateUserProfileCommandHandler : BaseRequestHandler<UpdateUserProfileCommand, Result<bool>>
    {
        public UpdateUserProfileCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }
        public override async Task<Result<bool>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = _userState.ID;
            if (string.IsNullOrEmpty(userId))
            {
                return Result.Failure<bool>(UserErrors.UserNotAuthenticated);
            }

            var userResult = await _mediator.Send(new GetUserByIdQuery(int.Parse(userId)));

            var user = request.Map(userResult.Data);

            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);

        }
    }
}
