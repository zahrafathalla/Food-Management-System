using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Recipes.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;

namespace FoodApp.Api.CQRS.Recipes.Commands;

public record DeleteRecipeCommand(int RecipeId) : IRequest<Result<bool>>;

public class DeleteRecipeCommandHandler : BaseRequestHandler<DeleteRecipeCommand, Result<bool>>
{
    public DeleteRecipeCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

    public override async Task<Result<bool>> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipeResult = await _mediator.Send(new GetRecipeByIdQuery(request.RecipeId));
        if (!recipeResult.IsSuccess)
        {
            return Result.Failure<bool>(RecipeErrors.RecipeNotFound);
        }

        var recipe = recipeResult.Data;

        _unitOfWork.Repository<Recipe>().DeleteById(recipe.Id);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}

