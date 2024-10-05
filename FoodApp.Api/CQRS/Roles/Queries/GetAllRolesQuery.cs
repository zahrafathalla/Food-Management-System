using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using MediatR;

namespace FoodApp.Api.CQRS.Roles.Queries
{
    public record GetAllRolesQuery() : IRequest<Result<List<Role>>>;
    public class GetAllRolesQueryHandler : BaseRequestHandler<GetAllRolesQuery, Result<List<Role>>>
    {
        public GetAllRolesQueryHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public override async Task<Result<List<Role>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.Repository<Role>().GetAllAsync(); 
            return Result.Success(roles.ToList());
        }
    }
}
