using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Park
    {
        /// <summary>
        /// A surrogate key for the park
        /// </summary>
        public int ParkId { get; set; }
        /// <summary>
        /// The name of the park
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The location of the park
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// The date that the park was established
        /// </summary>
        public DateTime EstablishDate { get; set; }
        /// <summary>
        /// The size of the park in square kilometers
        /// </summary>
        public int Area { get; set; }
        /// <summary>
        /// The annual number of visitors to the park
        /// </summary>
        public int Visitors { get; set; }
        /// <summary>
        /// A short description about the park
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Prints formatted park information
        /// </summary>
        /// <returns>The formatted string of park information</returns>
        public override string ToString()
        {
            return Name + " National Park\n\n"
                + "Location:".PadRight(20) + Location + "\n"
                + "Established:".ToString().PadRight(20) + EstablishDate.ToString("MM/dd/yyyy") + "\n"
                + "Area:".ToString().PadRight(20) + Area.ToString("n0") + " sq km\n"
                + "Annual Visitors:".ToString().PadRight(20) + Visitors.ToString("n0") + "\n\n"
                + Description;
        }
    }
}
