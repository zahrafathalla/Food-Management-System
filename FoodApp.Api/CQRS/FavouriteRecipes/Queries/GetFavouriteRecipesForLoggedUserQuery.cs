﻿using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Categories.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using FoodApp.Api.Repository.Specification.RecipeSpec;
using MediatR;

namespace FoodApp.Api.CQRS.FavouriteRecipes.Queries
{
    public record GetFavouriteRecipesForLoggedUserQuery() : IRequest<Result<List<FavouriteRecipToReturnDto>>>;

    public record FavouriteRecipToReturnDto(int Id, string Name);

    public class GetFavouriteRecipesForLoggedUserQueryHandler : BaseRequestHandler<GetFavouriteRecipesForLoggedUserQuery, Result<List<FavouriteRecipToReturnDto>>>
    {
        public GetFavouriteRecipesForLoggedUserQueryHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public override async Task<Result<List<FavouriteRecipToReturnDto>>> Handle(GetFavouriteRecipesForLoggedUserQuery request, CancellationToken cancellationToken)
        {
            var userId = string.IsNullOrEmpty(_userState.ID) ? 0 : int.Parse(_userState.ID);

            if (userId == 0)
            {
                return Result.Failure<List<FavouriteRecipToReturnDto>>(UserErrors.UserNotAuthenticated);
            }

            var spec = new FavouriteRecipesWithSpecification(userId);
            var favouriteRecipes = await _unitOfWork.Repository<FavouriteRecipe>().ListAsync(spec);

            if (favouriteRecipes == null || !favouriteRecipes.Any())
            {
                return Result.Failure<List<FavouriteRecipToReturnDto>>(FavouriteRecipeErrors.FavouriteRecipeNotFound);
            }

            var favouriteRecipesDto = favouriteRecipes
                .Select(fr => new FavouriteRecipToReturnDto(fr.Recipe.Id, fr.Recipe.Name))
                .ToList();

            return Result.Success(favouriteRecipesDto);
        }
    }
}
