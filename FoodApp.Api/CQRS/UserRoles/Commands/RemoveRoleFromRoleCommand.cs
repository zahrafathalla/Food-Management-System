using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Account.Queries;
using FoodApp.Api.CQRS.Roles.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;

namespace FoodApp.Api.CQRS.UserRoles.Commands
{
    public record RemoveRoleFromUserCommand(int UserId, int RoleId) : IRequest<Result<bool>>;
    public class RemoveRoleFromUserCommandHandler : BaseRequestHandler<RemoveRoleFromUserCommand, Result<bool>>
    {
        public RemoveRoleFromUserCommandHandler(RequestParameters requestParameters) : base(requestParameters)
        {
        }

        public override async Task<Result<bool>> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var roleResult = await _mediator.Send(new GetRoleByIdQuery(request.RoleId));
            if (!roleResult.IsSuccess)
            {
                return Result.Failure<bool>(RoleErrors.RoleNotFound);
            }
            var role = roleResult.Data;

            var userResult = await _mediator.Send(new GetUserByIdQuery(request.UserId));
            if (!userResult.IsSuccess)
            {
                return Result.Failure<bool>(UserErrors.UserNotFound);
            }
            var user = userResult.Data;


            var userRole = (await _unitOfWork.Repository<UserRole>()
                .GetAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId)).FirstOrDefault();

            if (userRole == null)
            {
                return Result.Failure<bool>(RoleErrors.RoleNotAssigned);
            }

            _unitOfWork.Repository<UserRole>().Delete(userRole);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
