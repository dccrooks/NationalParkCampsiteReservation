using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private string connectionString;
        /// <summary>
        /// Creates ReservationSqlDAO object
        /// </summary>
        /// <param name="databaseconnectionString">Database connection string</param>
        public ReservationSqlDAO(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }
        /// <summary>
        /// Adds reservation
        /// </summary>
        /// <param name="siteID">Site ID for the reservation</param>
        /// <param name="name">Name for the reservation</param>
        /// <param name="arrivalDate">Arrival date for the reservation</param>
        /// <param name="departureDate">Departure date for the reservation</param>
        /// <returns>Reservation ID if successful, 0 if failed</returns>
        public int AddReservation(int siteID, string name, DateTime arrivalDate, DateTime departureDate)
        {
            int reserveId = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation "
                        + "(site_id, name, from_date, to_date) "
                        + "VALUES(@site_id, @name, @from_date, @to_date); "
                        + "SELECT SCOPE_IDENTITY();",
                        connection);
                    cmd.Parameters.AddWithValue("@site_id", siteID);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@from_date", arrivalDate);
                    cmd.Parameters.AddWithValue("@to_date", departureDate);
                    reserveId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred adding reservation data to the database.");
                Console.WriteLine(e.Message);
                throw;
            }

            return reserveId;
        }
        /// <summary>
        /// Converts SQL reader data to Reservation object
        /// </summary>
        /// <param name="reader">SQL reader data</param>
        /// <returns>Reservation object</returns>
        private Reservation ConvertReaderToReservations(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();

            reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
            reservation.SiteId = Convert.ToInt32(reader["site_id"]);
            reservation.Name = Convert.ToString(reader["name"]);
            reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
            reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
            reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);

            return reservation;
        }
    }
}
