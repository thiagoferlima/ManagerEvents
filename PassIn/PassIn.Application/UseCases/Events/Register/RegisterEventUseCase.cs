using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Data;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public ResponseRegisteredJson Execute(RequestEventJson request) 
        {
            Validate(request);

            var DbContext = new PassInDbContext();

            var entity = new Infrastructure.Entities.Event
            {
                Title = request.Title,
                Details = request.Details,
                Maximum_Attendees = request.MaximumAttendees,
                Slug = request.Title.ToLower().Replace(" ", "-"),
            };
            DbContext.Events.Add(entity);
            DbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                Id = entity.Id
            };
        }
        private void Validate(RequestEventJson request) 
        {
            if (request.MaximumAttendees <= 0) 
            {
                throw new ErrorOnValidationException("The Maximum attends is invalid.");
            }
            if(string.IsNullOrWhiteSpace(request.Title)) 
            {
                throw new ErrorOnValidationException("the title is invalid.");
            }
            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new ErrorOnValidationException("the details is invalid.");
            }
        }
    }
}
