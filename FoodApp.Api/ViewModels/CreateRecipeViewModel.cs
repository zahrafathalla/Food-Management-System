using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using MediatR;

namespace FoodApp.Api.ViewModels;
public record CreateRecipeViewModel(
    string Name,
    IFormFile ImageUrl,
    decimal Price,
    string Description,
    int CategoryId);