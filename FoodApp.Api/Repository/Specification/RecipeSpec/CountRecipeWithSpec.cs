using FoodApp.Api.Data.Entities;

namespace FoodApp.Api.Repository.Specification.RecipeSpec.RecipeSpec;

public class CountRecipeWithSpec : BaseSpecification<Recipe>
{
    public CountRecipeWithSpec(SpecParams specParams)
        : base(p => !p.IsDeleted)
    {

    }
}