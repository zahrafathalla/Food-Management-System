using FoodApp.Api.Helper;
using FoodApp.Api.Repository.Interface;
using MediatR;

namespace FoodApp.Api.DTOs
{
    public class RequestParameters
    {
        public EmailSenderHelper EmailSenderHelper { get; set; }
        public IMediator Mediator { get; set; }
        public UserState UserState { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }

        public RequestParameters(IMediator mediator, UserState userState, IUnitOfWork unitOfWork, EmailSenderHelper emailSenderHelper)
        {
            Mediator = mediator;
            UserState = userState;
            UnitOfWork = unitOfWork;
            EmailSenderHelper = emailSenderHelper;
        }
    }
}
