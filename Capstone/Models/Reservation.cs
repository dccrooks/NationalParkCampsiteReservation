using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        /// <summary>
        /// A surrogate key for the reservation
        /// </summary>
        public int ReservationId { get; set; }
        /// <summary>
        /// The campsite the reservation is for
        /// </summary>
        public int SiteId { get; set; }
        /// <summary>
        /// The name for the reservation
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The start date of the reservation
        /// </summary>
        public DateTime FromDate { get; set; }
        /// <summary>
        /// The end date of the reservation
        /// </summary>
        public DateTime ToDate { get; set; }
        /// <summary>
        /// The date the reservation was booked
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Prints formatted reservation information
        /// </summary>
        /// <returns>The formatted string of reservation information</returns>
        public override string ToString()
        {
            return Name.ToString().PadRight(6) + FromDate.ToString().PadRight(30) + ToDate.ToString().PadRight(30);
        }
    }
}
