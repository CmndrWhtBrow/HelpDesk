using HelpDesk.Models;

namespace HelpDesk.Services.Abstractions
{
    public interface ITechnicianService
    {
        Technician AddTechnician(string name, TechnicianRole role);
        bool AssignTechnicianToTicket(int ticketId, int technicianId);
        Technician GetTechnician(int technicianId);
        bool RemoveTechnician(int technicianId, int? reassignedTechnicianId = null);
        bool RemoveTechnicianFromTicket(int ticketId, int technicianId);
        bool UpdateTechnician(int technicianId, string name, TechnicianRole role);
    }
}