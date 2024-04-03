using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Requests;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendees
{
    public class RegisterAttendeeOnEventUseCase
    {

        private readonly PassInDbContext _dbContext;

        public RegisterAttendeeOnEventUseCase()
        {
            _dbContext = new PassInDbContext();
        }
        public void Execute(Guid eventId, RequestRegisterEventJson request) 
        {
           
            Validate(eventId, request);

            var entity = new Infrastructure.Entities.Attendee
            {
                Email = request.Email,
                Name = request.Name,
                Event_Id = eventId,
                Created_At = DateTime.UtcNow,
      
            };
            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();
        }
        private void Validate(Guid eventId, RequestRegisterEventJson request) 
        {
            var existEvent = _dbContext.Events.Any(ev => ev.Id == eventId);
            if (existEvent == false)         
                throw new NotFoundException("An event with this id dont exist.");
            
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("the name is invalid.");
            }

            var emailIsValid = EmailIsIvalid(request.Email);
            if(emailIsValid == false) 
            {
                throw new ErrorOnValidationException("The e-mail is invalid.");
            }
            var attendeeAlreadyRegistered = _dbContext.Attendees.Any(attendee => attendee.Email.Equals(request.Email) && attendee.Event_Id == eventId);
            if(attendeeAlreadyRegistered) 
            {
                throw new ErrorOnValidationException("You can not register twice on the same event.");
            }
        }
        private bool EmailIsIvalid(string email) 
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch 
            {
                return false;
            }
        }

    }
}
