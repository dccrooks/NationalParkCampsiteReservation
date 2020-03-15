using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
    public class NationalParkCLI
    {
        /// <summary>
        /// Menu command shortcuts
        /// </summary>
        const string Command_Parks_Quit = "q";
        const string Command_ParkInfo_ViewCampgrounds = "1";
        const string Command_ParkInfo_ReturnToPreviousScreen = "2";
        const string Command_Campgrounds_SearchForAvailableReservations = "1";
        const string Command_Campgrounds_ReturnToPreviousScreen = "2";
        const string Command_Reservations_Cancel = "0";

        /// <summary>
        /// Global divider line properties
        /// </summary>
        const int lineWidth = 80;
        const char lineChar = '_';
        const char dottedChar = '-';

        /// <summary>
        /// SQL DAOs
        /// </summary>
        private IParkDAO parkDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;

        /// <summary>
        /// CLI constructor
        /// </summary>
        /// <param name="parkDAO">Park SQL DAO</param>
        /// <param name="campgroundDAO">Campground SQL DAO</param>
        /// <param name="siteDAO">Site SQL DAO</param>
        /// <param name="reservationDAO">Reservation SQL DAO</param>
        public NationalParkCLI(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        /// <summary>
        /// Main CLI (View Parks Interface)
        /// </summary>
        public void RunCLI()
        {
            IList<Park> parks = parkDAO.GetParks();
            bool invalidCommand = false;

            while (true)
            {
                Console.Clear();

                PrintParksHeader();
                PrintParksMenu(parks);

                invalidCommand = PrintInvalidCommand(invalidCommand);
                string command = Console.ReadLine().ToLower().Trim();
                bool validInt = Int32.TryParse(command, out int parkSelection);

                if (validInt && parkSelection >= 1 && parkSelection <= parks.Count)
                {
                    RunParkInfoCLI(parks[parkSelection - 1]);
                }
                else
                {
                    switch (command)
                    {
                        case Command_Parks_Quit:
                            Console.Clear();
                            Console.WriteLine("Thank you for using the National Park Campsite Reservation System!\n");
                            return;
                        default:
                            invalidCommand = true;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Park Information CLI
        /// </summary>
        /// <param name="park">The selected park from the main CLI</param>
        private void RunParkInfoCLI(Park park)
        {
            bool invalidCommand = false;

            while (true)
            {
                Console.Clear();

                PrintParkInfoHeader();
                PrintParkInfo(park);
                PrintParkInfoMenu();

                invalidCommand = PrintInvalidCommand(invalidCommand);
                string command = Console.ReadLine().Trim();

                switch (command)
                {
                    case Command_ParkInfo_ViewCampgrounds:
                        RunCampgroundsCLI(park);
                        break;
                    case Command_ParkInfo_ReturnToPreviousScreen:
                        return;
                    default:
                        invalidCommand = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Campgrounds CLI
        /// </summary>
        /// <param name="park">Park object selection</param>
        private void RunCampgroundsCLI(Park park)
        {
            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(park.ParkId);
            bool invalidCommand = false;

            while (true)
            {
                Console.Clear();

                PrintCampgroundsHeader();
                PrintCampgrounds(campgrounds, park.Name);
                PrintCampgroundsMenu();

                invalidCommand = PrintInvalidCommand(invalidCommand);
                string command = Console.ReadLine().Trim();

                switch (command)
                {
                    case Command_Campgrounds_SearchForAvailableReservations:
                        RunReservationsCLI(campgrounds, park.Name);
                        break;
                    case Command_Campgrounds_ReturnToPreviousScreen:
                        return;
                    default:
                        invalidCommand = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Site reservations CLI
        /// </summary>
        /// <param name="campgrounds">Campground to filter by</param>
        /// <param name="parkName">Park name</param>
        private void RunReservationsCLI(IList<Campground> campgrounds, string parkName)
        {
            bool invalidCommand = false;

            while (true)
            {
                Console.Clear();

                PrintReservationsHeader();
                PrintCampgrounds(campgrounds, parkName);
                Console.WriteLine();

                invalidCommand = PrintInvalidCommand(invalidCommand);
                Console.Write("Which campground (enter 0 to cancel)? ");
                string command = Console.ReadLine().Trim();
                bool validInt = Int32.TryParse(command, out int campgroundSelection);

                if (validInt && campgroundSelection >= 1 && campgroundSelection <= campgrounds.Count)
                {
                    RunSiteSearch(campgrounds[campgroundSelection - 1]);
                }
                else
                {
                    switch (command)
                    {
                        case Command_Reservations_Cancel:
                            return;
                        default:
                            invalidCommand = true;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Searches for sites available for reservation given arrival and departure dates
        /// </summary>
        /// <param name="campground">The campground to filter sites by</param>
        private void RunSiteSearch(Campground campground)
        {
            bool found = false;

            while (!found)
            {
                DateTime arrivalDate = GetDate(campground.OpenFromMonth, campground.OpenToMonth, "arrival", DateTime.Now);
                DateTime departureDate = GetDate(campground.OpenFromMonth, campground.OpenToMonth, "depature", arrivalDate);
                IList<Site> sites = GetAvailableSites(campground, arrivalDate, departureDate);

                if (sites.Count > 0)
                {
                    found = true;
                    BookReservation(sites, arrivalDate, departureDate);
                }
                else
                {
                    string answer;

                    do
                    {
                        Console.Write("No results found. Would you like to enter an alternate date range? (Y or N) ");
                        answer = Console.ReadLine().Trim().ToUpper();
                    } while (answer != "Y" && answer != "N");

                    if (answer == "N")
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Books a site reservation
        /// </summary>
        /// <param name="sites">List of available sites</param>
        /// <param name="arrivalDate">Arrival date for the reservation</param>
        /// <param name="departureDate">Departure date for the reservation</param>
        private void BookReservation(IList<Site> sites, DateTime arrivalDate, DateTime departureDate)
        {
            int siteIndex = -1;

            do
            {
                int siteNumber = CLIHelper.GetInteger("Which site should be reserved (enter 0 to cancel)? ");

                if (siteNumber == 0)
                {
                    Console.Clear();
                    return;
                }

                siteIndex = sites.IndexOf(sites.Where(x => x.SiteNumber == siteNumber).FirstOrDefault());
            } while (siteIndex < 0);

            string name = CLIHelper.GetString("What name should the reservation be made under? ");

            int reservationId = reservationDAO.AddReservation(sites[siteIndex].SiteId, name, arrivalDate, departureDate);

            Console.WriteLine("".PadRight(lineWidth, lineChar));

            if (reservationId > 0)
            {
                Console.WriteLine("The reservation has been made and the confirmation id is {" + reservationId + "}.\n");
            }
            else
            {
                Console.WriteLine("An error occurred making the reservation.");
            }

            Console.WriteLine("Press any key to return to the previous menu");
            Console.ReadKey();
        }

        /// <summary>
        /// Gets list of sites available for reservation given arrival and departure dates
        /// </summary>
        /// <param name="campground">Campground to filter sites by</param>
        /// <param name="arrivalDate">Arrival date for the reservation</param>
        /// <param name="departureDate">Departure date for the reservation</param>
        /// <returns></returns>
        private IList<Site> GetAvailableSites(Campground campground, DateTime arrivalDate, DateTime departureDate)
        {
            Console.WriteLine("".PadRight(lineWidth, lineChar));
            Console.WriteLine("Results Matching Your Search Criteria\n");
            Console.WriteLine("Site No.".PadRight(12) + "Max Occup.".PadRight(14) + "Accessible?".PadRight(15) + "Max RV Length".PadRight(17) + "Utility".PadRight(12) + "Cost".PadRight(10));
            Console.WriteLine("".PadRight(lineWidth, dottedChar));

            IList<Site> sites = siteDAO.GetAvailableSites(campground.CampgroundId, arrivalDate, departureDate);

            for (int i = 0; i < sites.Count; i++)
            {
                decimal cost = (decimal)(departureDate - arrivalDate).TotalDays * campground.DailyFee;
                Console.WriteLine(sites[i].ToString() + $"{cost:C2}".ToString().PadRight(10));
            }

            Console.WriteLine();

            return sites;
        }

        /// <summary>
        /// Gets arrival and departure dates from user
        /// </summary>
        /// <param name="fromMonth">Minimum month allowed for reservations at campground</param>
        /// <param name="toMonth">Maximum month allowed for reservations at campground</param>
        /// <param name="type">Whether the date is arrival or departure</param>
        /// <param name="minimumDate">Minimum date allowed for reservation</param>
        /// <returns>The arrival or departure date</returns>
        private DateTime GetDate(int fromMonth, int toMonth, string type, DateTime minimumDate)
        {
            DateTime date = DateTime.Now;
            bool validDate = false;

            while (!validDate)
            {
                date = CLIHelper.GetDateTime("What is the " + type + " date? ");

                if (date > minimumDate)
                {
                    if (date.Year == DateTime.Now.Year)
                    {
                        if (date.Month >= fromMonth && date.Month <= toMonth)
                        {
                            validDate = true;
                        }
                        else
                        {
                            Console.WriteLine("The campground is not open for reservations during the month entered. Please try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Reservations are only being accepted for the current year. Please try again.");
                    }
                }
                else
                {
                    if (type == "arrival")
                    {
                        Console.WriteLine("The arrival date must be later than today's date. Please try again.");
                    }
                    else
                    {
                        Console.WriteLine("The departure date must be later than the arrival date. Please try again.");
                    }
                }
            }

            return date;
        }

        /// <summary>
        /// Prints park information for a selected park
        /// </summary>
        /// <param name="park">Park object selection</param>
        private void PrintParkInfo(Park park)
        {
            Console.WriteLine("".PadRight(lineWidth, lineChar));
            Console.WriteLine(park.ToString());
        }

        /// <summary>
        /// Prints list of campgrounds for a specific park
        /// </summary>
        /// <param name="campgrounds">The list of campgrounds</param>
        /// <param name="parkName">The park name</param>
        private void PrintCampgrounds(IList<Campground> campgrounds, string parkName)
        {
            Console.WriteLine("".PadRight(lineWidth, lineChar));
            Console.WriteLine(parkName + " National Park Campgrounds\n");
            Console.WriteLine("".PadRight(6) + "Name".PadRight(40) + "Open".PadRight(12) + "Close".PadRight(12) + "Daily Fee".PadRight(10));
            Console.WriteLine("".PadRight(lineWidth, dottedChar));

            for (int i = 0; i < campgrounds.Count; i++)
            {
                Console.WriteLine(("#" + (i + 1)).PadRight(6) + campgrounds[i].ToString());
            }
        }

        /// <summary>
        /// Prints header for main CLI (View Parks Interface)
        /// </summary>
        private void PrintParksHeader()
        {
            Console.WriteLine("NATIONAL PARK CAMPSITE RESERVATION SYSTEM");
        }

        /// <summary>
        /// Prints header for Park Information CLI
        /// </summary>
        private void PrintParkInfoHeader()
        {
            Console.WriteLine("PARK INFORMATION");
        }

        /// <summary>
        /// Prints header for Campgrounds CLI
        /// </summary>
        private void PrintCampgroundsHeader()
        {
            Console.WriteLine("PARK CAMPGROUNDS");
        }

        /// <summary>
        /// Prints header for Reservations CLI
        /// </summary>
        private void PrintReservationsHeader()
        {
            Console.WriteLine("SEARCH FOR CAMPGROUND RESERVATION");
        }

        /// <summary>
        /// Prints menu of all parks
        /// </summary>
        /// <param name="parks">List of park objects</param>
        private void PrintParksMenu(IList<Park> parks)
        {
            Console.WriteLine("".PadRight(lineWidth, lineChar));
            Console.WriteLine("Select a Park for further details:\n");

            for (int i = 0; i < parks.Count; i++)
            {
                Console.WriteLine((i + 1) + ") " + parks[i].Name);
            }

            Console.WriteLine("   ----");
            Console.WriteLine("Q) Quit\n");
        }

        /// <summary>
        /// Prints menu options for Park Information CLI
        /// </summary>
        private void PrintParkInfoMenu()
        {
            Console.WriteLine("".PadRight(lineWidth, lineChar));
            Console.WriteLine("Select a Command:\n");
            Console.WriteLine("1) View Campgrounds");
            Console.WriteLine("2) Return to Previous Screen\n");
        }

        /// <summary>
        /// Prints menu options for Campgrounds CLI
        /// </summary>
        private void PrintCampgroundsMenu()
        {
            Console.WriteLine("".PadRight(lineWidth, lineChar));
            Console.WriteLine("Select a Command:\n");
            Console.WriteLine("1) Search for Available Reservation");
            Console.WriteLine("2) Return to Previous Screen\n");
        }

        /// <summary>
        /// Prints invalid command message
        /// </summary>
        /// <param name="invalidCommand">Whether the entered command is invalid</param>
        /// <returns>Resets invalid boolean to false</returns>
        private bool PrintInvalidCommand(bool invalidCommand)
        {
            if (invalidCommand)
            {
                Console.WriteLine("Invalid command. Please try again.\n");
            }

            return false;
        }
    }
}
