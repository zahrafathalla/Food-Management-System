using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.CQRS.Users.Queries
{
    public record GetUserProfileQuery() : IRequest<Result<UserToReturnDto>>;

    public class GetUserProfileQueryHandler : BaseRequestHandler<GetUserProfileQuery, Result<UserToReturnDto>>
    {
        public GetUserProfileQueryHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public async  override Task<Result<UserToReturnDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _userState.ID;
            if(string.IsNullOrEmpty(userId))
            {
                return Result.Failure<UserToReturnDto>(UserErrors.UserNotAuthenticated);

            }
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(int.Parse(userId));

            if (user == null)
            {
                return Result.Failure<UserToReturnDto>(UserErrors.UserNotFound);
            }

            var mappedUser = user.Map<UserToReturnDto>();

            return Result.Success(mappedUser);
        }
    }
}
