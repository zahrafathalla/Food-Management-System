using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Discounts.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.CQRS.Discounts.Commands
{
    public record DeleteDiscountCommand(int DiscountId) : IRequest<Result<bool>>;

    public class DeleteDiscountCommandHandler : BaseRequestHandler<DeleteDiscountCommand, Result<bool>>
    {
        public DeleteDiscountCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public async override Task<Result<bool>> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var discountResult = await _mediator.Send(new GetDiscountByIdQuery(request.DiscountId));
            var discount = discountResult.Data;

            if (discount == null)
            {
                return Result.Failure<bool>(DiscountErrors.DiscountNotFound);
            }

            var discountRepo = _unitOfWork.Repository<Discount>();
            discountRepo.Delete(discount);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }

}
