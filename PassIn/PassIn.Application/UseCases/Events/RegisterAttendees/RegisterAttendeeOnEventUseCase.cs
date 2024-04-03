using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PassIn.Communication.Requests;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.RegisterAttendees
{
    public class RegisterAttendeeOnEventUseCase
    {
        public void Execute(Guid eventId, RequestRegisterEventJson request) 
        {
            var DbContext = new PassInDbContext();

        }
        private void Validate(Guid eventId, RequestRegisterEventJson request, PassInDbContext dbContext) 
        {
            var eventExist = dbContext.Events.Any(ev => ev .Id == eventId);
            if (eventExist == false)
            {
                throw new NotFoundException("An event with this id dont exist.");
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("the name is invalid.");
            }


        }

    }
}
