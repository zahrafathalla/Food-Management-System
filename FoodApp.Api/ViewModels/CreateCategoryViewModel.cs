using System.ComponentModel.DataAnnotations;

namespace FoodApp.Api.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
