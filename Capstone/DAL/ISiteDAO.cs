using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISiteDAO
    {
        /// <summary>
        /// Gets all available sites for a specific campground and arrival/departure dates
        /// </summary>
        /// <param name="campgroundId">The campground ID to filter by</param>
        /// <param name="arrivalDate">The arrival date</param>
        /// <param name="departureDate">The departure date</param>
        /// <returns></returns>
        IList<Site> GetAvailableSites(int campgroundId, DateTime arrivalDate, DateTime departureDate);
    }
}
