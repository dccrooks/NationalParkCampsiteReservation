using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Capstone.Models
{
    public class Campground
    {
        /// <summary>
        /// A surrogate key for the campground
        /// </summary>
        public int CampgroundId { get; set; }
        /// <summary>
        /// The park that the campground is associated with
        /// </summary>
        public int ParkId { get; set; }
        /// <summary>
        /// The name of the campground
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The numerical month the campground is open for reservation
        /// </summary>
        public int OpenFromMonth { get; set; }
        /// <summary>
        /// The numerical month the campground is closed for reservation
        /// </summary>
        public int OpenToMonth { get; set; }
        /// <summary>
        /// The daily fee for booking a campsite at this campground
        /// </summary>
        public decimal DailyFee { get; set; }
        /// <summary>
        /// Prints formatted campground information
        /// </summary>
        /// <returns>The formatted string of campground information</returns>
        public override string ToString()
        {
            return Name.ToString().PadRight(40)
                + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(OpenFromMonth).PadRight(12)
                + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(OpenToMonth).PadRight(12)
                + $"{DailyFee:C2}".PadRight(10);
        }
    }
}
