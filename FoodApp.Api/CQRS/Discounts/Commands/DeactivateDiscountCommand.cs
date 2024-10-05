using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;

namespace FoodApp.Api.CQRS.Discounts.Commands
{
    public record DeactivateDiscountCommand(int DiscountId) : IRequest<Result<bool>>;

    public class DeactivateDiscountCommandHandler : BaseRequestHandler<DeactivateDiscountCommand, Result<bool>>
    {
        public DeactivateDiscountCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public async override Task<Result<bool>> Handle(DeactivateDiscountCommand request, CancellationToken cancellationToken)
        {
            var discountRepo = _unitOfWork.Repository<Discount>();
            var discount = await discountRepo.GetByIdAsync(request.DiscountId);

            if (discount == null)
            {
                return Result.Failure<bool>(DiscountErrors.DiscountNotFound);
            }

            discount.EndDate = DateTime.UtcNow;

            discountRepo.Update(discount);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }


}
