using FoodApp.Api.Abstraction;
using MediatR;

namespace FoodApp.Api.ViewModels;

public record UpdateRecipeViewModel(
    int RecipeId,
    string Name,
    IFormFile ImageUrl,
    decimal Price,
    string Description,
    int CategoryId);