using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using FoodApp.Api.Repository.Specification;
using FoodApp.Api.Repository.Specification.RecipeSpec;
using FoodApp.Api.Response;
using MediatR;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.CQRS.Recipes.Queries;

public record GetRecipesQuery(SpecParams SpecParams) : IRequest<Result<IEnumerable<RecipeToReturnDto>>>;

public class GetRecipesQueryHandler : BaseRequestHandler<GetRecipesQuery, Result<IEnumerable<RecipeToReturnDto>>>
{
    public GetRecipesQueryHandler(RequestParameters requestParameters) : base(requestParameters) { }

    public override async Task<Result<IEnumerable<RecipeToReturnDto>>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
    {
        var spec = new RecipeSpecification(request.SpecParams);
        var recipe = await _unitOfWork.Repository<Recipe>().GetAllWithSpecAsync(spec);


        if (recipe == null)
        {
            return Result.Failure<IEnumerable<RecipeToReturnDto>>(RecipeErrors.RecipeNotFound);
        }

        var response = recipe.Map<IEnumerable<RecipeToReturnDto>>();

        return Result.Success(response);
    }
}