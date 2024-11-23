using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transport_Management_System_App.Models;
using Transport_Management_System_App.util;
using Transport_Management_System_App.myexceptions;
namespace Transport_Management_System_App.Repository
{

    internal class TransportManagementServiceImpl : TransportManagementService
    {
       
        //string connectionString = "Server=DESKTOP-6CA5ALC; Database=Transport Management System; Trusted_Connection=True";
        public bool addVehicle(Vehicles vehicle)
        {
            string query = "insert into vehicles values(@model, @capacity, @type, @status)";

            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@model", vehicle.Model);
                    cmd.Parameters.AddWithValue("@capacity", vehicle.Capacity);
                    cmd.Parameters.AddWithValue("@type", vehicle.Type);
                    cmd.Parameters.AddWithValue("@status", vehicle.Status);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }

            return false;
        }


        public void updateVehicle(int vehicleId,Vehicles vehicle)
        {
            //if(!checkVehicle(vehicleId))
            //{
            //    return false;
            //}

            StringBuilder query = new StringBuilder("UPDATE vehicles SET model = @model, capacity = @capacity, type = @type, status = @status ");
            query.Append("WHERE vehicleID = @vehicleID");
            String query_s = query.ToString();
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query_s, conn))
                {
                    cmd.Parameters.AddWithValue("@model", vehicle.Model);
                    cmd.Parameters.AddWithValue("@capacity", vehicle.Capacity);
                    cmd.Parameters.AddWithValue("@type", vehicle.Type);
                    cmd.Parameters.AddWithValue("@status", vehicle.Status);
                    cmd.Parameters.AddWithValue("@vehicleID", vehicleId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (!(rowsAffected > 0))
                    {
                        throw new VehicleNotFoundException("Vehicle not found Exception!");
                        
                    }
                }
            }
            
        }
        public static bool checkVehicle(int id)
        {
            String query = "select * from vehicles where vehicleId = @vehicleId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleId", id);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return true;
                        }
                        
                    }
       
                   
                }
            }
            return false;
        }


        public bool deleteVehicle(int vehicleId)
        {
            string query = "delete from vehicles where vehicleID = @vehicleID";
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleID", vehicleId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if(rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        throw new VehicleNotFoundException($"Vehicle not found exception vehicle ID : {vehicleId}");
                    }
                }
            }

        }
        //d
        //f

        public bool cancelTrip(int tripId)
        {
            string query = "delete from trips where tripID = @tripID";
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tripID", tripId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }


        //h,i

        public List<Bookings> getBookingByPassenger(int passengerId)
        {
            List<Bookings> bookings = new List<Bookings>();

            string query = "select * from Bookings where passengerID = @passengerID";

            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@passengerID", passengerId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Bookings booking_detail = new Bookings();
                        booking_detail.BookingID = (int)reader["bookingID"];
                        booking_detail.TripID = (int)reader["tripID"];
                        booking_detail.PassengerID = (int)reader["passengerID"];
                        booking_detail.BookingDate = (String)reader["bookingDate"];
                        booking_detail.Status = (string)reader["status"];
                        bookings.Add(booking_detail);
                    }
                }
            }

            return bookings;

        }



        public List<Bookings> getBookingsByTrip(int tripId)
        {
            List<Bookings> bookings = new List<Bookings>();

            string query = "select * from Bookings where tripID = @tripId";

            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tripId", tripId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Bookings booking_detail = new Bookings();
                        booking_detail.BookingID = (int)reader["bookingID"];
                        booking_detail.TripID = (int)reader["tripID"];
                        booking_detail.PassengerID = (int)reader["passengerID"];
                        booking_detail.BookingDate = (String)reader["bookingDate"];
                        booking_detail.Status = (string)reader["status"];
                        bookings.Add(booking_detail);

                    }
                }
            }

            return bookings;

        }

        public void cancelBooking(int bookingId)
        {
            String query = "delete from bookings where bookingID = @bookingID ";
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@bookingID", bookingId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if(!(rowsAffected>0))
                    {
                        throw new BookingNotFoundException($"Booking ID : {bookingId} not found exception !");
                    }
                }
            }
            
        }



        public bool scheduleTrip(int vehicleId, int routeId, string departureDate, string arrivalDate)
        {
            String query = null;
            if(!vehicleAvailable(vehicleId))
            {
                Console.WriteLine("Vehicle Not Found !");
                return false;
            }
            query = "update vehicles set status = 'On Trip' where vehicleId = @vehicleId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleId",vehicleId);
                    cmd.ExecuteNonQuery();
                }
            }
            

            query = "insert into trips values(@vehicleId,@routeId,@departureDate,@arrivalDate,'Scheduled','passenger',50,1)";
            using( SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleId", vehicleId);
                    cmd.Parameters.AddWithValue("@routeId",routeId);
                    cmd.Parameters.AddWithValue("@departureDate",departureDate);
                    cmd.Parameters.AddWithValue("@arrivalDate",arrivalDate);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }


        }

        // helper method for schedule Trip
        public static bool vehicleAvailable(int vehicleId)
        {
            String query = "Select status from vehicles where vehicleId = @vehicleId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleId",vehicleId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            String val = (String)reader["status"];
                            if (val == "Available")
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool bookTrip(int tripId, int passengerId, string bookingDate)
        {
            if(!TripAvailable(tripId))
            {
                Console.WriteLine("Trip not available !");
                return false;
            }
            if(!PassengerAvailable(passengerId))
            {
                Console.WriteLine("passenger not available");
                return false;
            }

            String query = "insert into bookings values(@tripId , @passengerId,@bookingDate , 'confirmed')";
            using( SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using( SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tripId",tripId);
                    cmd.Parameters.AddWithValue("@passengerId",passengerId);
                    cmd.Parameters.AddWithValue("@bookingDate",bookingDate);
                    int rowsAffected =  cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }

            return false;

        }

        // Helper class for bookTrip , check the existence of tripId
        public static bool TripAvailable(int tripId)
        {
            String query = "select * from trips where tripId = @tripId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@tripId",tripId);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if( reader.Read())
                        {
                            String val = (String)reader["status"];
                            if(val == "Scheduled")
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        // Helper class for bookTrip , check the existence of passenger
        public static bool PassengerAvailable(int passengerId)
        {
            String query = "select * from Passengers where passengerId = @passengerId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@passengerId",passengerId);
                    cmd.ExecuteNonQuery();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        

        public bool allocateDriver(int tripId, int driverId)
        {
            if(!TripAvailable(tripId))
            {
                Console.WriteLine("Trip not available !");
                return false;
            }
            if(!DriverAvailable(driverId))
            {
                Console.WriteLine("Driver not available");
                return false;
            }

            String query = "update trips set driverId = @driverId where tripId = @tripId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@driverId",driverId);
                    cmd.Parameters.AddWithValue("@tripId",tripId);
                    cmd.ExecuteNonQuery();
                }
            }
            query = "update driver set status = 1 where driverId = @driverId";
            using( SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using( SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@driverId",driverId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }

            return false;
        }

        // Helper method for allocate Driver method , check the driver is avaialble or not
        public static bool DriverAvailable(int driverId)
        {
            String query = "select * from Driver where driverId = @driverId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@driverId",driverId);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            int status = (int)reader["status"];
                            if(status == 0)
                                return true;
                        }
                    }
                }
            }
            return false;

        }

        public bool deallocateDriver(int tripId)
        {
            int driverId = 0;
            String query = "Select driverId from trips where tripId = @tripId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@tripId", tripId);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                             driverId = (int)reader["driverId"];
                        else
                        {
                            Console.WriteLine("No driver present !");
                            return false;
                        }
                    }
                }
            }

            query = "update trips set driverId = null where tripId = @tripId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {

                    cmd.Parameters.AddWithValue("@tripId", tripId);
                    

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            driverId = (int)reader["driverId"];
                    }
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (!(rowsAffected > 0))
                    {
                        return false;
                    }
                }
            }
            query = "update Driver set status = 0 where driverId = @driverId";
            using (SqlConnection con = DBConnection.getConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@driverId", driverId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }

        }

        // driver id is foreign key , so do (on delete set null) , manually assign null , we cannt assign null to foreign key

        public List<Driver> getAvailableDrivers()
        {
            List<Driver> drivers = new List<Driver>();
            string query = "select * from Driver where status = 0";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Driver driver_detail = new Driver();
                            driver_detail.DriverName = (string)reader["DriverName"];
                            driver_detail.DriverId = (int)reader["driverId"];
                            driver_detail.Status = (int)reader["status"];
                            drivers.Add(driver_detail);
                        }
                        
                    }
                    
                }
            }
            return drivers;
        }

       





    }
}
