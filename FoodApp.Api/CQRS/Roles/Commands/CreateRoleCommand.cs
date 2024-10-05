using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Roles.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.CQRS.Roles.Commands
{
    public record CreateRoleCommand(string roleName) : IRequest<Result<bool>>;
    public class CreateRoleCommandHandler : BaseRequestHandler<CreateRoleCommand, Result<bool>>
    {
        public CreateRoleCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public override async Task<Result<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var roleResult = await _mediator.Send(new GetRoleByNameQuery(request.roleName), cancellationToken);

            if (roleResult is not null)
            {
                return Result.Failure<bool>(RoleErrors.RoleAlreadyExists);
            }
            var role = request.Map<Role>();
            var roleRepo = _unitOfWork.Repository<Role>();
            await roleRepo.AddAsync(role);
            await roleRepo.SaveChangesAsync();

            return Result.Success(true);

        }
    }
}
