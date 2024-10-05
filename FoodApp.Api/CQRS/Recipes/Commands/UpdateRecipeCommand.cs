﻿using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Recipes.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using FoodApp.Api.Helper;
using MediatR;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.CQRS.Recipes.Commands;

public record UpdateRecipeCommand(
    int RecipeId,
    string Name,
    IFormFile ImageUrl,
    decimal Price,
    string Description,
    int CategoryId) : IRequest<Result<bool>>;

public class UpdateRecipeCommandHandler : BaseRequestHandler<UpdateRecipeCommand, Result<bool>>
{
    public UpdateRecipeCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

    public override async Task<Result<bool>> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipeResult = await _mediator.Send(new GetRecipeByIdQuery(request.RecipeId));
        if (!recipeResult.IsSuccess)
        {
            return Result.Failure<bool>(RecipeErrors.RecipeNotFound);
        }


        var recipe = request.Map(recipeResult.Data);

        _unitOfWork.Repository<Recipe>().Update(recipe);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}