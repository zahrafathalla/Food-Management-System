using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;

namespace FoodApp.Api.CQRS.Roles.Queries
{
    public record GetRoleByNameQuery(string Name) : IRequest<Result<Role>>;
    public class GetRoleByNameQueryHandler : BaseRequestHandler<GetRoleByNameQuery, Result<Role>>
    {
        public GetRoleByNameQueryHandler(RequestParameters requestParameters) : base(requestParameters) { }
        public override async Task<Result<Role>> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
        {
            var role = (await _unitOfWork.Repository<Role>().GetAsync(u => u.Name == request.Name)).FirstOrDefault();
            if (role == null)
            {
                return null;
            }

            return Result.Success(role);
        }
    }
}
