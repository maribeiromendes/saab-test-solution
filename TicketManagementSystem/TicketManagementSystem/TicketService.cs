using System;
using EmailService;

namespace TicketManagementSystem
{
    public class TicketService
    {
        public int CreateTicket(string title, Priority priority, string assignedUser, string description, DateTime creationDateTime, bool isPayingCustomer)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || assignedUser == null)
                throw new InvalidTicketException("Title or description or assignedUser were null");

            User user = GetUser(assignedUser);

            var ticket = new Ticket(title, user, description, creationDateTime, priority);

            ticket.RaisePriority();
            
            if (isPayingCustomer)
            {
                ticket.AdjustPrice();

                User accountManager = new UserRepository().GetAccountManager();
                ticket.SetAccountManager(accountManager);
            }

            if (ticket.Priority == Priority.High)
            {
                var emailService = new EmailServiceProxy();
                emailService.SendEmailToAdministrator(title, assignedUser);
            }

            var id = TicketRepository.CreateTicket(ticket);

            return id;
        }

        public void AssignTicket(int ticketId, string assignedUser)
        {
            User user = GetUser(assignedUser);

            var ticket = TicketRepository.GetTicket(ticketId);

            if (ticket == null)
            {
                throw new ApplicationException("No ticket found for id " + ticketId);
            }

            ticket.SetAssignedUser(user);

            TicketRepository.UpdateTicket(ticket);
        }

        private User GetUser(string assignedUser)
        {
            using (var userRepository = new UserRepository())
            {
                var user = userRepository.GetUser(assignedUser);

                if (user == null)
                    throw new UnknownUserException("User " + assignedUser + " not found");

                return user;
            }
        }
    }
}
