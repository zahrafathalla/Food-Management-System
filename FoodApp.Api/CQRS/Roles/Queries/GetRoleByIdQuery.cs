using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;

namespace FoodApp.Api.CQRS.Roles.Queries
{
    public record GetRoleByIdQuery(int RoleId) : IRequest<Result<Role>>;
    public class GetRoleByIdQueryHandler : BaseRequestHandler<GetRoleByIdQuery, Result<Role>>
    {
        public GetRoleByIdQueryHandler(RequestParameters requestParameters) : base(requestParameters)
        {
        }

        public override async Task<Result<Role>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.Repository<Role>().GetByIdAsync(request.RoleId);
            if (role == null)
            {
                return Result.Failure<Role>(RoleErrors.RoleNotFound);
            }

            return Result.Success(role);
        }
    }
}
