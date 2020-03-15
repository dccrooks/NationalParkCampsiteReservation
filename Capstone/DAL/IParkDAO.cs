using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public interface IParkDAO
    {
        /// <summary>
        /// Gets all parks
        /// </summary>
        /// <returns>List of Park objects</returns>
        IList<Park> GetParks();
    }
}
