﻿using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.FavouriteRecipes.Queries;
using FoodApp.Api.CQRS.Recipes.Queries;
using FoodApp.Api.CQRS.Users.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;

namespace FoodApp.Api.CQRS.FavouriteRecipes.Commands
{
    public record AddRecipeToFavouritesCommand(int RecipeId) : IRequest<Result<bool>>;
    public class AddRecipeToFavouritesCommandHandler : BaseRequestHandler<AddRecipeToFavouritesCommand, Result<bool>>
    {
        public AddRecipeToFavouritesCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public override async Task<Result<bool>> Handle(AddRecipeToFavouritesCommand request, CancellationToken cancellationToken)
        {
            var userId = string.IsNullOrEmpty(_userState.ID) ? 0 : int.Parse(_userState.ID); ;
            if (userId == 0)
            {
                return Result.Failure<bool>(UserErrors.UserNotAuthenticated);
            }

            var recipeResult = await _mediator.Send(new GetRecipeByIdQuery(request.RecipeId), cancellationToken);
            if (!recipeResult.IsSuccess)
            {
                return Result.Failure<bool>(recipeResult.Error);
            }
            var recipe = recipeResult.Data;

            var favouriteRecipeResult = await _mediator.Send(new GetFavouriteRecipeByUserIdAndRecipeIdQuery(userId, request.RecipeId), cancellationToken);

            if (favouriteRecipeResult.IsSuccess)
            {
                return Result.Failure<bool>(FavouriteRecipeErrors.FavouriteRecipeAlreadyExists);
            }

            var favouriteRecipe = new FavouriteRecipe
            {
                UserId = userId,
                RecipeId = recipe.Id
            };

            await _unitOfWork.Repository<FavouriteRecipe>().AddAsync(favouriteRecipe);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
