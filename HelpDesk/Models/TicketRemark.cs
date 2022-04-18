namespace HelpDesk.Models
{
    public class TicketRemark
    {
        public int Id { get; set; }
        public int TechnicianId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
