using AutoMapper;
using FoodApp.Api.CQRS.Account.Commands;
using FoodApp.Api.CQRS.Account.Orchestrator;
using FoodApp.Api.CQRS.Categories.Commands;
using FoodApp.Api.CQRS.Categories.Queries;
using FoodApp.Api.CQRS.Discounts.Commands;
using FoodApp.Api.CQRS.Discounts.Queries;
using FoodApp.Api.CQRS.FavouriteRecipes.Commands;
using FoodApp.Api.CQRS.FavouriteRecipes.Queries;
using FoodApp.Api.CQRS.Recipes.Commands;
using FoodApp.Api.CQRS.Roles.Commands;
using FoodApp.Api.CQRS.UserRoles.Commands;
using FoodApp.Api.CQRS.Users.Commands;
using FoodApp.Api.CQRS.Users.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.Helper.RecipeUrlResolve;
using FoodApp.Api.Response;
using FoodApp.Api.ViewModels;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;

namespace FoodApp.Api.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            /***************  Account  ***************/

            CreateMap<RegisterCommand, User>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<RegisterViewModel, RegisterCommand>();

            CreateMap<LoginViewModel, LoginCommand>();

            CreateMap<ChangePasswordViewModel, ChangePasswordCommand>();
            CreateMap<ForgotPasswordViewModel, ForgotPasswordCommand>();
            CreateMap<ResetPasswordViewModel, ResetPasswordCommand>();
            CreateMap<RegisterOrchestrator, RegisterCommand>();
            CreateMap<RegisterViewModel, RegisterOrchestrator >();
            CreateMap<VerifyViewModel, VerifyOTPCommand>();

            //user
            CreateMap<User, UserToReturnDto>();
            CreateMap<UpdateUserProfileCommand, User>()
                  .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateUserProfileViewModel, UpdateUserProfileCommand>();

            /***************  Role  ***************/


            CreateMap<CreateRoleCommand, Role>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.roleName)); 

            CreateMap<CreateRoleViewModel, CreateRoleCommand>();
            CreateMap<AssignRoleToUserViewModel, AddRoleToUserCommand>();

            CreateMap<User, LoginResponse>()
               .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src =>
                   src.RefreshTokens
                      .Where(r => r.IsActive)
                      .Select(r => r.Token)
                      .FirstOrDefault()));

            /***************  Recipe  ***************/

            CreateMap<CreateRecipeCommand, Recipe>()
                 .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                 .AfterMap(async (src, dest) =>
                 {
                     dest.ImageUrl = await DocumentSettings.UploadFileAsync(src.ImageUrl, "Images");
                 });

            CreateMap<CreateRecipeViewModel, CreateRecipeCommand>();

            CreateMap<UpdateRecipeViewModel, UpdateRecipeCommand>();

            CreateMap<UpdateRecipeCommand, Recipe>()
                 .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                 .AfterMap(async (src, dest) =>
                 {
                     dest.ImageUrl = await DocumentSettings.UploadFileAsync(src.ImageUrl, "Images");
                 });

            CreateMap<Recipe, RecipeToReturnDto>()
                  .ForMember(dest => dest.Discount, opt => opt.MapFrom(src =>src.RecipeDiscounts
                  .Where(rd => rd.Discount != null && rd.Discount.IsActive)
                  .Select(rd => rd.Discount.DiscountPercent)
                 .FirstOrDefault())).ReverseMap();
            // Discount 

            CreateMap<UpdateDiscountCommand, Discount>()
             .ForMember(dest => dest.DiscountPercent, opt => opt.Condition(src => src.DiscountPercent.HasValue))
             .ForMember(dest => dest.StartDate, opt => opt.Condition(src => src.StartDate.HasValue))
             .ForMember(dest => dest.EndDate, opt => opt.Condition(src => src.EndDate.HasValue));

            CreateMap<AddDiscountCommand, Discount>();
            CreateMap<DiscountToReturnDto, Discount>().ReverseMap();
            CreateMap<AddDiscountViewModel, AddDiscountCommand>();
            CreateMap<ApplyDiscountViewModel, ApplyDiscountCommand>();

            //category

            CreateMap<CreateCategoryViewModel, CreateCategoryCommand>();
            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<CategoryToReturnDto, Category>().ReverseMap();
            CreateMap<Recipe, RecipesNamesToReturnDto>();

            //FavouriteRecipe
            CreateMap<AddRecipeToFavouritesViewModel, AddRecipeToFavouritesCommand>();
            CreateMap<AddRecipeToFavouritesCommand, FavouriteRecipe>().ReverseMap();
            CreateMap<FavouriteRecipToReturnDto, FavouriteRecipe>().ReverseMap();

        }
    }
}
