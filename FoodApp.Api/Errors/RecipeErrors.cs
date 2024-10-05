using ProjectManagementSystem.Errors;

namespace FoodApp.Api.Errors;

public class RecipeErrors
{

    public static readonly Error RecipeNotFound =
        new("Recipe is not found", StatusCodes.Status404NotFound);
}