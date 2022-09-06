using System;

namespace TicketManagementSystem
{
    public class Ticket
    {
        public Ticket(string title, User user, string description, DateTime creationDateTime, Priority priority)
        {
            Title = title;
            AssignedUser = user;
            Description = description;
            Created = creationDateTime;
            Priority = priority;
            PriceDollars = 0;
        }

        public int Id { get; set; }

        public string Title { get; private set; }

        public Priority Priority { get; private set; }

        public string Description { get; private set; }

        public User AssignedUser { get; private set; }

        public User AccountManager { get; private set; }

        public DateTime Created { get; private set; }

        public double PriceDollars { get; private set; }

        public void SetAssignedUser(User user)
        {
            AssignedUser = user;
        }

        public void RaisePriority()
        {
            if ((Created < DateTime.UtcNow - TimeSpan.FromHours(1)) || (Title.Contains("Crash") || Title.Contains("Important") || Title.Contains("Failure")))
            {
                switch (Priority)
                {
                    case Priority.Low:
                        Priority = Priority.Medium;
                        break;
                    case Priority.Medium:
                        Priority = Priority.High;
                        break;
                }
            }
        }

        public void AdjustPrice()
        {
            PriceDollars = Priority == Priority.High ? 100 : 50;
        }

        public void SetAccountManager(User accountManager)
        {
            AccountManager = accountManager;
        }
    }
}
