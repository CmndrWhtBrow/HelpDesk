namespace HelpDesk.Models
{
    public class DataState
    {
        public List<Technician> Technicians = new();
        public List<Ticket> Tickets { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public List<TicketRemark> TicketRemarks = new();
    }
}
