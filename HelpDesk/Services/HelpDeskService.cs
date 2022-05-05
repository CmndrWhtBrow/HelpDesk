using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDesk.Models;
using HelpDesk.Models.Abstractions;
using HelpDesk.Services.Abstractions;

namespace HelpDesk.Services
{
    public class HelpDeskService
    {
        private IDataContext _dataContext;
        private readonly ITechnicianService? _technicianService;
        private readonly IUserService? _userService;
        private readonly Menu? _menu;


        //public void Run()
        //{
        //    RunMainMenu();
        //}
        public void RunMainMenu()
        {
            _menu?.MainMenu();
            string selection = Console.ReadLine();

            if (selection == "1") // User
            {
                RunUser();
            }

            else if (selection == "2") // Technician
            {
                RunTechnician();
            }

            else if (selection == "3") // Exit
            {
                Console.WriteLine("Good-bye!");
            }

            else
            {
                Console.WriteLine("Invalid selection");
                RunMainMenu();
            }
        }

        public void RunUser()
        {
            Console.Clear();
            _menu?.User();
            string selection = Console.ReadLine();

            if (selection == "1") //select user  DONE
            {
                if (_dataContext.CurrentState.Users == null)
                {
                    Console.WriteLine("There are no users at this time, please create a new User.");
                    Console.WriteLine("Enter name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Create a UserName: ");
                    string userName = Console.ReadLine();
                    Console.WriteLine("Please enter your email address:");
                    string email = Console.ReadLine();
                    _userService?.AddUser(name, userName, email);
                    Console.WriteLine("New user has been created.");
                    Console.ReadLine();
                    Console.Clear();
                }

                Console.WriteLine("Displaying Users");
                Console.WriteLine("");
                _userService?.ViewUsers();

                RunUserMenu();

            }
            else if (selection == "2") // create new user  DONE
            {
                Console.WriteLine("Please enter your name:");
                string name = Console.ReadLine();
                Console.WriteLine("Create a UserName: ");
                string userName = Console.ReadLine();
                Console.WriteLine("Please enter your email address:");
                string email = Console.ReadLine();
                _userService?.AddUser(name, userName, email);

                Console.ReadLine();
                RunUser();

            }
            else  // Main Menu
            {
                RunMainMenu();
            }

        }

        public void RunUserMenu()
        {
            int userId = Convert.ToInt32(Console.ReadLine());

            _menu?.UserMenu();
            string selection = Console.ReadLine();

            if (selection == "1") // create ticket    //TODO: select ticket priority
            {
                Console.WriteLine("Create a trouble ticket:");
                Console.WriteLine("Enter subject:");
                string subject = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("Enter Description:");
                string description = Console.ReadLine();
                //How to allow user to select the ticketPriority?

                _userService?.AddTicket(userId, subject, description, TicketPriority.Lowest);
                RunUserMenu();
            }
            else if (selection == "2") // view MY open tickets   DONE
            {
                _userService?.ViewUserTickets(userId);
                Console.ReadLine();
                RunUserMenu();
            }
            else if (selection == "3") //Settings
            {
                Console.Clear();

                _menu.TicketSettings();

                string update = Console.ReadLine();

                if (update == "1")   // Update User   DONE
                {
                    Console.WriteLine("Update User info");
                    Console.WriteLine("Enter name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter UserName:");
                    string userName = Console.ReadLine();
                    Console.WriteLine("Enter email address:");
                    string email = Console.ReadLine();

                    _userService?.UpdateUser(userId, name, userName, email);
                    Console.WriteLine("User info has been updated.");
                    Console.ReadLine();
                    RunUserMenu();
                }

                else if (update == "2")  // Delete Ticket   DONE
                {
                    System.Console.WriteLine("");
                    _userService?.ViewUserTickets(userId);
                    System.Console.WriteLine("Select a Ticket to delete:");
                    int ticketId = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Are you sure you want to delete this ticket? ");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");
                    selection = Console.ReadLine();
                    if (selection == "1")
                    {
                        Console.WriteLine("Ticket has been deleted");
                        _userService?.RemoveTicket(userId, ticketId);
                    }

                    RunUserMenu();

                }
                else if (update == "3")  // Delete User   DONE
                {
                    Console.WriteLine("Delete user account?");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");
                    selection = Console.ReadLine();
                    if (selection == "1")
                    {
                        Console.WriteLine("Are you sure you want to delete this user account?");
                        Console.WriteLine("1. Yes");
                        Console.WriteLine("2. No");
                        selection = Console.ReadLine();
                        if (selection == "1")
                        {
                            Console.WriteLine("User has been deleted.");
                            _userService.RemoveUser(userId);
                            Console.ReadLine();
                            RunMainMenu();
                        }

                    }

                    RunUserMenu();
                }
                else //Return
                {
                    RunUser();
                }
            }
            else
            {
                RunMainMenu();
            }
        }

        public void RunTechnician()  // DONE
        {
            Console.Clear();

            if (_dataContext.CurrentState.Technicians == null)
            {
                Console.WriteLine("There are no Technicians at this time. Please create a new Technican.");
                _dataContext.CurrentState.Technicians = new List<Technician>();
                Console.WriteLine("Enter name: ");
                string name = Console.ReadLine();
                // how to define TechnicianRole role?
                _technicianService?.AddTechnician(name, TechnicianRole.EntryLevel);
            }

            _menu?.Technician();
            string selection = Console.ReadLine();

            if (selection == "1") // select technician DONE
            {
                RunTechnicianMenu();
            }

            else if (selection == "2") //create new tech   DONE (except technician role)
            {
                Console.Clear();
                Console.WriteLine("Enter technician name:");
                string name = Console.ReadLine();
                Console.WriteLine("Enter job title");
                string role = Console.ReadLine();
                _technicianService?.AddTechnician(name, TechnicianRole.EntryLevel);  // How to define Technician Role?

                RunTechnician();
            }

            else  //main menu
            {
                RunMainMenu();
            }


        }

        public void RunTechnicianMenu()  //**DONE
        {

            Console.Clear();
            _technicianService?.ViewTechnicians();
            int technicianId = Convert.ToInt32(Console.ReadLine());

            _menu?.TechMenu();
            string selection = Console.ReadLine();

            if (selection == "1") // view my tickets   DONE
            {
                _technicianService?.GetTechnicianTickets(technicianId);
                Console.WriteLine("Select a Ticket");
                int ticketId = Convert.ToInt32(Console.ReadLine());

                RunTicketMenu(technicianId, ticketId);
            }
            else if (selection == "2") // view open tickets  DONE
            {
                _technicianService?.ViewOpenTickets();
                Console.WriteLine("Would you like to assign yourself a ticket?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. Return");
                selection = Console.ReadLine();
                if (selection == "1")
                {
                    Console.WriteLine("Select the ticket number you wish to be assigned:");
                    int ticketId = Convert.ToInt32(Console.ReadLine());
                    _technicianService?.AssignTechnicianToTicket(ticketId, technicianId);
                }

                RunTechnicianMenu();

            }
            else if (selection == "3") // settings  **DONE
            {
                RunTechnicianSettings(technicianId);
            }
            else // return
            {
                RunTechnician();
            }


        }

        public void RunTicketMenu(int technicianId, int ticketId)  //TODO: ADD RESPONSE
        {
            _menu?.TicketMenu();
            string selection = Console.ReadLine();
            if (selection == "1") // add response
            {
                // HOW TO?
            }
            else if (selection == "2") // mark resolved  DONE
            {
                _technicianService?.IsCompleted(ticketId, technicianId);
                Console.WriteLine("Ticket is complete.");
                Console.ReadLine();
                RunTechnicianMenu();
            }
            else if (selection == "3")  // reassign  DONE
            {
                Console.WriteLine("Who would you like to reassign this to?");
                int reassignTechnician = Convert.ToInt32(Console.ReadLine());
                _technicianService?.RemoveTechnicianFromTicket(ticketId, technicianId);
                _technicianService?.AssignTechnicianToTicket(ticketId, reassignTechnician);

                Console.WriteLine("Ticket has been reassigned");
                Console.ReadLine();
                RunTechnicianMenu();
            }
            else
            {
                RunTechnicianMenu();
            }
        }

        public void RunTechnicianSettings(int techId)  // DONE (except Technician Role)
        {
            _menu?.SettingsMenu();
            string selection = Console.ReadLine();
            if (selection == "1") //update bio
            {
                Console.WriteLine("Enter name");
                string name = Console.ReadLine();
                Console.WriteLine("Enter position");
                string role = Console.ReadLine(); //how to select Technician Role? Switch?

                _technicianService?.UpdateTechnician(techId, name, TechnicianRole.EntryLevel);

                RunTechnicianMenu();
            }
            else if (selection == "2") //delete technician   
            {
                Console.WriteLine("Would you like to remove this technician?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");
                selection = Console.ReadLine();
                if (selection == "1")
                {

                    Console.WriteLine("Are you sure you want to remove this technician?");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");
                    selection = Console.ReadLine();
                    if (selection == "1")
                    {
                        Console.WriteLine("Before you delete your account, would you like to select a technician to reassign your tickets to?");
                        Console.WriteLine("1. Yes");
                        Console.WriteLine("2. No");
                        selection= Console.ReadLine();
                        if (selection == "1")
                        {
                            _technicianService?.ViewTechnicians();
                            Console.WriteLine("Please select a technician.");
                            int reassignedTechnicianId = Convert.ToInt32(Console.ReadLine());
                            _technicianService?.RemoveTechnician(techId, reassignedTechnicianId);
                        }
                        else
                        {
                            _technicianService?.RemoveTechnician(techId);                         
                        }
                        Console.WriteLine("Technician has been removed.");
                        Console.ReadLine();
                        RunTechnician();
                    }

                    RunTechnicianMenu();


                }

                RunTechnicianMenu();

            }
            else
            {
                RunTechnicianMenu();
            }
        }
    }
}
