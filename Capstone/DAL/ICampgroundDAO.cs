using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public interface ICampgroundDAO
    {
        /// <summary>
        /// Gets all campgrounds for a specific park
        /// </summary>
        /// <param name="parkID">The park ID to filter by</param>
        /// <returns>List of Campground objects</returns>
        IList<Campground> GetCampgrounds(int parkID);
    }
}
