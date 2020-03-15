using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string connectionString;
        /// <summary>
        /// Creates CampgroundSqlDAO object
        /// </summary>
        /// <param name="databaseconnectionString">Database connection string</param>
        public CampgroundSqlDAO(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }
        /// <summary>
        /// Gets all campgrounds for a specific park
        /// </summary>
        /// <param name="parkID">The park ID to filter by</param>
        /// <returns>List of Campground objects</returns>
        public IList<Campground> GetCampgrounds(int parkID)
        {
            List<Campground> campgrounds = new List<Campground>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * "
                        + "FROM campground "
                        + "WHERE park_id = @park_id "
                        + "ORDER BY name;",
                        connection);
                    cmd.Parameters.AddWithValue("@park_id", parkID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        campgrounds.Add(ConvertReaderToCampground(reader));
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred retrieving campground data from the database.");
                Console.WriteLine(e.Message);
                throw;
            }

            return campgrounds;
        }
        /// <summary>
        /// Converts SQL reader data to Campground object
        /// </summary>
        /// <param name="reader">SQL reader data</param>
        /// <returns>Campground object</returns>
        private Campground ConvertReaderToCampground(SqlDataReader reader)
        {
            Campground campground = new Campground();

            campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            campground.ParkId = Convert.ToInt32(reader["park_id"]);
            campground.Name = Convert.ToString(reader["name"]);
            campground.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
            campground.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
            campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

            return campground;
        }
    }
}
