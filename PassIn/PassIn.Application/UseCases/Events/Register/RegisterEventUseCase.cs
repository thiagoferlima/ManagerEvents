using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Requests;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Data;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public void Execute(RequestEventJson request) 
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
        }
        private void Validate(RequestEventJson request) 
        {
            if (request.MaximumAttendees <= 0) 
            {
                throw new PassInException("The Maximum attends is invalid.");
            }
            if(string.IsNullOrWhiteSpace(request.Title)) 
            {
                throw new PassInException("the title is invalid.");
            }
            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new PassInException("the details is invalid.");
            }
        }
    }
}
