using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteDAO
    {
        private string connectionString;
        /// <summary>
        /// Creates SiteSqlDAO object
        /// </summary>
        /// <param name="databaseconnectionString">Database connection string</param>
        public SiteSqlDAO(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }
        /// <summary>
        /// Gets all available sites for a specific campground and arrival/departure dates
        /// </summary>
        /// <param name="campgroundId">The campground ID to filter by</param>
        /// <param name="arrivalDate">The arrival date</param>
        /// <param name="departureDate">The departure date</param>
        /// <returns></returns>
        public IList<Site> GetAvailableSites(int campgroundId, DateTime arrivalDate, DateTime departureDate)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TOP 5 * "
                        + "FROM site "
                        + "WHERE campground_id = @campground_id "
                        + "AND site_id NOT IN "
                        + "(SELECT site.site_id "
                        + "FROM site "
                        + "JOIN reservation ON site.site_id = reservation.site_id "
                        + "WHERE campground_id = @campground_id "
                        + "AND @from_date BETWEEN from_date AND to_date "
                        + "OR @to_date BETWEEN from_date AND to_date)", connection);
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                    cmd.Parameters.AddWithValue("@from_date", arrivalDate);
                    cmd.Parameters.AddWithValue("@to_date", departureDate);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        sites.Add(ConvertReaderToSite(reader));
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred retrieving site data from the database.");
                Console.WriteLine(e.Message);
                throw;
            }

            return sites;
        }
        /// <summary>
        /// Converts SQL reader data to Site object
        /// </summary>
        /// <param name="reader">SQL reader data</param>
        /// <returns>Site object</returns>
        private Site ConvertReaderToSite(SqlDataReader reader)
        {
            Site site = new Site();

            site.SiteId = Convert.ToInt32(reader["site_id"]);
            site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            site.SiteNumber = Convert.ToInt32(reader["site_number"]);
            site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.Accessible = Convert.ToBoolean(reader["accessible"]);
            site.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
            site.Utilities = Convert.ToBoolean(reader["utilities"]);

            return site;
        }
    }
}
