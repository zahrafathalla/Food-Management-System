using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Discounts.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.CQRS.Discounts.Commands
{
    public record UpdateDiscountCommand(int DiscountId, decimal? DiscountPercent, DateTime? StartDate, DateTime? EndDate) : IRequest<Result<bool>>;

    public class UpdateDiscountCommandHandler : BaseRequestHandler<UpdateDiscountCommand, Result<bool>>
    {
        public UpdateDiscountCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public async override Task<Result<bool>> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {

            var discountResult =await _mediator.Send(new GetDiscountByIdQuery(request.DiscountId));
            var discount = discountResult.Data;

            if (discount == null)
            {
                return Result.Failure<bool>(DiscountErrors.DiscountNotFound);
            }

            if (request.DiscountPercent <= 0 || request.DiscountPercent > 100)
            {
                return Result.Failure<bool>(DiscountErrors.DiscountPercentageNotValid);
            }

            if (request.EndDate <= request.StartDate)
            {
                return Result.Failure<bool>(DiscountErrors.DatesNotValid);
            }

            var updatedDiscount = request.Map(discount);

            var discountRepo = _unitOfWork.Repository<Discount>();
            discountRepo.Update(updatedDiscount);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }

}
