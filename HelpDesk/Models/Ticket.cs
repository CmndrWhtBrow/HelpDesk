using Newtonsoft.Json;

namespace HelpDesk.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int UserId { get; set; }
        public int? TechnicianId { get; set; }
        public DateTime? AssignedAt { get; set; }
        public bool IsAssigned => AssignedAt != null;
        public DateTime? CompletedAt { get; set; }
        public bool IsCompleted => CompletedAt != null;
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted => DeletedAt != null;
        [JsonIgnore]
        public List<TicketRemark> Remarks { get; set; } = new List<TicketRemark>();
    }
}
