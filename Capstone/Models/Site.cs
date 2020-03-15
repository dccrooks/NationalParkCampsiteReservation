using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        /// <summary>
        /// A surrogate key for the campsite
        /// </summary>
        public int SiteId { get; set; }
        /// <summary>
        /// The campground that the park belongs to
        /// </summary>
        public int CampgroundId { get; set; }
        /// <summary>
        /// The arbitrary campsite number
        /// </summary>
        public int SiteNumber { get; set; }
        /// <summary>
        /// Maximum occupancy at the campsite
        /// </summary>
        public int MaxOccupancy { get; set; }
        /// <summary>
        /// Indicates whether or not the campsite is handicap accessible
        /// </summary>
        public bool Accessible { get; set; }
        /// <summary>
        /// The maximum RV length that the campsite can fit.
        /// 0 indicates that no RV will fit at this campsite.
        /// </summary>
        public int MaxRvLength { get; set; }
        /// <summary>
        /// Indicates whether or not the campsite provides access to utility hookup
        /// </summary>
        public bool Utilities { get; set; }
        /// <summary>
        /// Prints formatted site information
        /// </summary>
        /// <returns>The formatted string of site information</returns>
        public override string ToString()
        {
            string accessible = Accessible ? "Yes" : "No";
            string maxRvLength = MaxRvLength > 0 ? MaxRvLength.ToString() : "N/A";
            string utilities = Utilities ? "Yes" : "N/A";

            return SiteNumber.ToString().PadRight(12)
                + MaxOccupancy.ToString().PadRight(14)
                + accessible.PadRight(15)
                + maxRvLength.PadRight(17)
                + utilities.PadRight(12);
        }
    }
}
