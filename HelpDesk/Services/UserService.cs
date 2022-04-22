using HelpDesk.Models;
using HelpDesk.Models.Abstractions;
using HelpDesk.Services.Abstractions;

namespace HelpDesk.Services
{

    public class UserService
    {

        private IDataContext _dataContext;
        public UserService(IDataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        /*
       * Create Ticket -- DONE
       * Update User  -- DONE
       * Remove User -- DONE
       * Get User Tickets -- DONE
       * Get One Ticket -- DONE
       * Delete Ticket? -- DONE
       */

        public User AddUser( string name, string userName, string email)
        {
            try
            {
                if (_dataContext.CurrentState.Users == null)
                {
                    _dataContext.CurrentState.Users = new List<User>();
                }

                int userId = _dataContext.CurrentState.Users?.OrderByDescending(u => u.Id).FirstOrDefault()?.Id ?? 0;
                int newUserId = userId + 1;
                User newUser = new User() { Id = newUserId, Name = name, UserName = userName, Email = email };
                _dataContext.CurrentState.Users?.Add(newUser);
                _dataContext.SaveState();
                return newUser;

            }
            catch (Exception ex)
            {
                return null;
            }
        
        }

        public bool AddTicket ( int userId, string subject, string description, TicketPriority priority) //TODO
        {
            try
            {
                if (_dataContext.CurrentState.Tickets == null)
                {
                    _dataContext.CurrentState.Tickets = new List<Technician>();
                }

                int ticketId = _dataContext.CurrentState.Technicians?.OrderByDescending(t => t.Id).FirstOrDefault()?.Id ?? 0;
                int newticketId = ticketId + 1;
                Ticket newTicket = new Ticket() { CreatedAt = DateTime.Now, Id = newticketid, Subject = subject, Description = description, Priority = priority, UserId = userId };
                _dataContext.CurrentState.Tickets?.Add(newTicket);
                _dataContext.SaveState();
                return newTicket;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool UpdateUser(int userId, string name, string userName, string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    //Name needs to have actual value
                    return false;
                }
                if (_dataContext.CurrentState.Users == null)
                {
                    //No Users
                    return false;
                }

                User? user = _dataContext.CurrentState.Users.FirstOrDefault( u => u.Id == userID);
                if (user == null)
                {
                    //User does not exist
                    return false;
                }

                user.Name = name;
                user.UserName = userName;
                user.Email = email;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool RemoveUser(int userId )
        {
            try
            {
                var user = GetUser(userId); 
                if (user == null)
                {
                    return true;
                }
                User? dataUser = _dataContext.CurrentState.Users.FirstOrDefault(u => u.Id == userId);

                if (dataUser == null)
                {
                    return true;
                }

                if (user.Tickets.Any())
                {
                     user.Tickets.ForEach(ticket =>
                    {
                        var dataTicket = _dataContext.CurrentState.Tickets.FirstOrDefault(u => u.Id == ticket.Id);
                        if (dataTicket != null)
                        {
                            dataTicket.UserId = null;
                        }
                    });
                }
                _dataContext.CurrentState.Users.Remove(dataUser);
                _dataContext.SaveState();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public User GetUser (int userId)
        {
            try
            {
                if (_dataContext.CurrentState.Users != null)
                {
                    if (_dataContext.CurrentState.Users.Any())
                    {
                        User? user = _dataContext.CurrentState.Users.FirstOrDefault(u => u.Id == userId);
                        if (user != null)
                        {
                            if (_dataContext.CurrentState.Tickets != null)
                            {
                                if (_dataContext.CurrentState.Tickets.Any())
                                {
                                    List<Ticket> userTickets = _dataContext.CurrentState.Tickets?.Where(u => u.UserId == userId)?.ToList() ?? new List<Ticket>();
                                    user.Tickets = userTickets;
                                    return user;
                                }
                            }
                            return user;
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

        public User GetTicket(int userId, int ticketId) 
        {
            try
            {
                if (_dataContext.CurrentState.Users != null)
                {
                    if (_dataContext.CurrentState.Users.Any())
                    {
                        User? user = _dataContext.CurrentState.Users.FirstOrDefault(u => u.Id == userId);
                        if (user != null)
                        {
                            if (_dataContext.CurrentState.Tickets != null)
                            {
                                List<Ticket>? userTicket = _dataContext.CurrentState.Tickets?.FirstOrDefault(t => t.Id == ticketId);
                                user.Tickets = userTicket;
                                return user;

                            }
                            return user;
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


        public bool RemoveTicket(int userId, int ticketId) 
        {
            try
            {
                var user = GetUser(userId);
                if (user == null)
                {
                    return true;
                }
                User? dataUser = _dataContext.CurrentState.Users.FirstOrDefault(u => u.Id == userId);

                if (dataUser == null)
                {
                    return true;
                }

                if (user.Tickets.Any())
                {
                    user.Tickets.ForEach(ticket =>
                    {
                        var dataTicket = _dataContext.CurrentState.Tickets.FirstOrDefault(t => t.Id == ticketId);
                        if (dataTicket != null)
                        {
                            _dataContext.CurrentState.Tickets.Remove(dataTicket);

                        }
                    });
                }
                _dataContext.SaveState();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }







    }






}