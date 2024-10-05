using ProjectManagementSystem.Errors;

namespace FoodApp.Api.Errors
{
    public class FavouriteRecipeErrors
    {
        public static readonly Error FavouriteRecipeNotFound=
        new("FavouriteRecipe is not found", StatusCodes.Status404NotFound);

        public static readonly Error FavouriteRecipeAlreadyExists =
            new("FavouriteRecipe Already Exists", StatusCodes.Status409Conflict);
    }
}
