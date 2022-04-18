using HelpDesk.Models;
using HelpDesk.Models.Abstractions;
using HelpDesk.Services.Abstractions;

namespace HelpDesk.Services
{
    public class TechnicianService : ITechnicianService
    {
        private IDataContext _dataContext;
        public TechnicianService(IDataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        /*
         * Add Technician -- DONE
         * Assign Technician To Ticket -- DONE
         * Remove Technician From Ticket -- DONE
         * Update Technician -- DONE
         * Remove Technician -- DONE
         * Get Techinician -- DONE
         */
        public Technician AddTechnician(string name, TechnicianRole role)
        {
            try
            {
                if (_dataContext.CurrentState.Technicians == null)
                {
                    _dataContext.CurrentState.Technicians = new List<Technician>();
                }

                int technicianId = _dataContext.CurrentState.Technicians?.OrderByDescending(t => t.Id).FirstOrDefault()?.Id ?? 0;
                int newTechnicianId = technicianId + 1;
                Technician newTechnician = new Technician() { Id = newTechnicianId, Name = name, Role = role };
                _dataContext.CurrentState.Technicians?.Add(newTechnician);
                _dataContext.SaveState();
                return newTechnician;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool AssignTechnicianToTicket(int ticketId, int technicianId)
        {
            try
            {
                if (_dataContext.CurrentState.Tickets == null)
                {
                    //No tickets
                    return false;
                }

                Ticket? ticket = _dataContext.CurrentState.Tickets.FirstOrDefault(t => t.Id == ticketId);
                if (ticket == null)
                {
                    //Ticket does not exist
                    return false;
                }
                if (_dataContext.CurrentState.Technicians == null)
                {
                    //No Technicians
                    return false;
                }

                Technician? technician = _dataContext.CurrentState.Technicians.FirstOrDefault(t => t.Id == technicianId);
                if (technician == null)
                {
                    //Technician does not exist
                    return false;
                }

                ticket.TechnicianId = technicianId;
                ticket.AssignedAt = DateTime.Now;
                _dataContext.SaveState();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveTechnicianFromTicket(int ticketId, int technicianId)
        {
            try
            {
                if (_dataContext.CurrentState.Tickets == null)
                {
                    //No tickets
                    return false;
                }

                Ticket? ticket = _dataContext.CurrentState.Tickets.FirstOrDefault(t => t.Id == ticketId);
                if (ticket == null)
                {
                    //Ticket does not exist
                    return false;
                }
                if (_dataContext.CurrentState.Technicians == null)
                {
                    //No Technicians
                    return false;
                }

                Technician? technician = _dataContext.CurrentState.Technicians.FirstOrDefault(t => t.Id == technicianId);
                if (technician == null)
                {
                    //Technician does not exist
                    return false;
                }

                ticket.TechnicianId = null;
                _dataContext.SaveState();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateTechnician(int technicianId, string name, TechnicianRole role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    //Name needs to have actual value
                    return false;
                }
                if (_dataContext.CurrentState.Technicians == null)
                {
                    //No Technicians
                    return false;
                }

                Technician? technician = _dataContext.CurrentState.Technicians.FirstOrDefault(t => t.Id == technicianId);
                if (technician == null)
                {
                    //Technician does not exist
                    return false;
                }

                technician.Name = name;
                technician.Role = role;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool RemoveTechnician(int technicianId, int? reassignedTechnicianId = null)
        {
            try
            {
                var technician = GetTechnician(technicianId);
                if (technician == null)
                {
                    return true;
                }
                Technician? dataTechnician = _dataContext.CurrentState.Technicians.FirstOrDefault(t => t.Id == technicianId);
                if (dataTechnician == null)
                {
                    return true;
                }

                if (technician.Tickets.Any())
                {
                    int? newTechId = null;
                    if (reassignedTechnicianId != null)
                    {
                        var reassignedTechnician = _dataContext.CurrentState.Technicians.FirstOrDefault(t => t.Id == reassignedTechnicianId);
                        if (reassignedTechnician != null)
                        {
                            newTechId = reassignedTechnician.Id;
                        }
                    }
                    technician.Tickets.ForEach(ticket =>
                    {
                        var dataTicket = _dataContext.CurrentState.Tickets.FirstOrDefault(t => t.Id == ticket.Id);
                        if (dataTicket != null)
                        {
                            dataTicket.TechnicianId = newTechId;
                        }
                    });
                }
                _dataContext.CurrentState.Technicians.Remove(dataTechnician);
                _dataContext.SaveState();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Technician GetTechnician(int technicianId)
        {
            try
            {
                if (_dataContext.CurrentState.Technicians != null)
                {
                    if (_dataContext.CurrentState.Technicians.Any())
                    {
                        Technician? technician = _dataContext.CurrentState.Technicians.FirstOrDefault(t => t.Id == technicianId);
                        if (technician != null)
                        {
                            if (_dataContext.CurrentState.Tickets != null)
                            {
                                if (_dataContext.CurrentState.Tickets.Any())
                                {
                                    List<Ticket> technicianTickets = _dataContext.CurrentState.Tickets?.Where(t => t.TechnicianId == technicianId)?.ToList() ?? new List<Ticket>();
                                    technician.Tickets = technicianTickets;
                                    return technician;
                                }
                            }
                            return technician;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
