using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ParkSqlDAO : IParkDAO
    {
        private string connectionString;
        /// <summary>
        /// Creates ParkSqlDAO object
        /// </summary>
        /// <param name="databaseconnectionString">Database connection string</param>
        public ParkSqlDAO(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }
        /// <summary>
        /// Gets all parks
        /// </summary>
        /// <returns>List of Park objects</returns>
        public IList<Park> GetParks()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM park order by name;", connection);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        parks.Add(ConvertReaderToPark(reader));
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred retrieving park data from the database.");
                Console.WriteLine(e.Message);
                throw;
            }

            return parks;
        }
        /// <summary>
        /// Converts SQL reader data to Park object
        /// </summary>
        /// <param name="reader">SQL reader data</param>
        /// <returns>Park object</returns>
        private Park ConvertReaderToPark(SqlDataReader reader)
        {
            Park park = new Park();

            park.ParkId = Convert.ToInt32(reader["park_id"]);
            park.Name = Convert.ToString(reader["name"]);
            park.Location = Convert.ToString(reader["location"]);
            park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
            park.Area = Convert.ToInt32(reader["area"]);
            park.Visitors = Convert.ToInt32(reader["visitors"]);
            park.Description = Convert.ToString(reader["description"]);

            return park;
        }
    }
}
