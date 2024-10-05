using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;

namespace FoodApp.Api.CQRS.FavouriteRecipes.Queries
{
    public record GetFavouriteRecipeByUserIdAndRecipeIdQuery(int UserId, int RecipeId) : IRequest<Result<FavouriteRecipe>>;
    public class GetFavouriteRecipeByUserIdAndRecipeIdQueryHandler : BaseRequestHandler<GetFavouriteRecipeByUserIdAndRecipeIdQuery, Result<FavouriteRecipe>>
    {
        public GetFavouriteRecipeByUserIdAndRecipeIdQueryHandler(RequestParameters requestParameters) : base(requestParameters)
        {
        }

        public override async Task<Result<FavouriteRecipe>> Handle(GetFavouriteRecipeByUserIdAndRecipeIdQuery request, CancellationToken cancellationToken)
        {
            var favouriteRecipe =( await _unitOfWork.Repository<FavouriteRecipe>()
                .GetAsync(fr => fr.UserId == request.UserId && fr.RecipeId == request.RecipeId && fr.IsDeleted == false)).FirstOrDefault();
            if (favouriteRecipe == null)
            {
                return Result.Failure<FavouriteRecipe>(FavouriteRecipeErrors.FavouriteRecipeNotFound);
            }

            return Result.Success(favouriteRecipe);

        }
    }

}
