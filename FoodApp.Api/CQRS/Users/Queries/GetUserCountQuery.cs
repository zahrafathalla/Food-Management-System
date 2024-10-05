using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.Repository.Interface;
using FoodApp.Api.Repository.Specification.UsesrSpec;
using FoodApp.Api.Repository.Specification;
using MediatR;
using FoodApp.Api.DTOs;

namespace FoodApp.Api.CQRS.Users.Queries
{
    public record GetUserCountQuery(SpecParams SpecParams) : IRequest<Result<int>>;

    public class GetUserCountQueryHandler : BaseRequestHandler<GetUserCountQuery, Result<int>>
    {
        public GetUserCountQueryHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public async override Task<Result<int>> Handle(GetUserCountQuery request, CancellationToken cancellationToken)
        {
            var userSpec = new CountUserWithSpec(request.SpecParams);
            var count = await _unitOfWork.Repository<User>().GetCountWithSpecAsync(userSpec);

            return Result.Success(count);
        }
    }
}
