using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using FoodApp.Api.Repository.Specification.RecipeSpec;
using MediatR;

namespace FoodApp.Api.CQRS.Recipes.Queries;

public record GetRecipeByIdQuery(int RecipeId) : IRequest<Result<Recipe>>;

public class GetRecipeByIdQueryHandler : BaseRequestHandler<GetRecipeByIdQuery, Result<Recipe>>
{
    public GetRecipeByIdQueryHandler(RequestParameters requestParameters) : base(requestParameters) { }

    public override async Task<Result<Recipe>> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new RecipeSpecification(request.RecipeId);
        var recipe = await _unitOfWork.Repository<Recipe>().GetByIdWithSpecAsync(spec);
        if (recipe == null)
        {
            return Result.Failure<Recipe>(RecipeErrors.RecipeNotFound);
        }

        return Result.Success(recipe);
    }
}