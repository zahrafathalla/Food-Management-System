using AutoMapper;
using FoodApp.Api.CQRS.Recipes.Commands;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.Response;
using static System.Net.Mime.MediaTypeNames;

namespace FoodApp.Api.Helper.RecipeUrlResolve;

public class RecipePictureUrlResolve : IValueResolver<Recipe, RecipeToReturnDto, string>
{

    private readonly IConfiguration _configuration;

    public RecipePictureUrlResolve(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(Recipe source, RecipeToReturnDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.ImageUrl))
        {
            return $"{_configuration["ApiBaseUrl"]}Files/Images/{source.ImageUrl}";
        }
        return string.Empty;
    }
}