using HelpDesk.Models;

namespace HelpDesk.Services
{
    public interface IUserService
    {
        bool AddTicket(int userId, string subject, string description, TicketPriority priority);
        User AddUser(string name, string userName, string email);
        User GetTicket(int userId, int ticketId);
        User GetUser(int userId);
        bool RemoveTicket(int userId, int ticketId);
        bool RemoveUser(int userId);
        bool UpdateUser(int userId, string name, string userName, string email);
        bool ViewUsers();
        void ViewUserTickets(int userId);

    }
}