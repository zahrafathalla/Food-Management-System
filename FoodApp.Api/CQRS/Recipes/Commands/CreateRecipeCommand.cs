using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Roles.Commands;
using FoodApp.Api.CQRS.Roles.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;
using ProjectManagementSystem.Helper;
using System.Data;

namespace FoodApp.Api.CQRS.Recipes.Commands;

public record CreateRecipeCommand(
    string Name,
    IFormFile ImageUrl,
    decimal Price,
    string Description,
    int CategoryId) : IRequest<Result<bool>>;

public class CreateRecipeCommandHandler : BaseRequestHandler<CreateRecipeCommand, Result<bool>>
{
    public CreateRecipeCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

    public override async Task<Result<bool>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = request.Map<Recipe>();

        await _unitOfWork.Repository<Recipe>().AddAsync(recipe);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}