namespace HelpDesk.Models
{
    public class Technician
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public TechnicianRole Role { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}
