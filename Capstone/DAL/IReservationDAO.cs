using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        /// <summary>
        /// Adds reservation
        /// </summary>
        /// <param name="siteID">Site ID for the reservation</param>
        /// <param name="name">Name for the reservation</param>
        /// <param name="arrivalDate">Arrival date for the reservation</param>
        /// <param name="departureDate">Departure date for the reservation</param>
        /// <returns>Reservation ID if successful, 0 if failed</returns>
        int AddReservation(int siteID, string name, DateTime arrivalDate, DateTime departureDate);
    }
}
