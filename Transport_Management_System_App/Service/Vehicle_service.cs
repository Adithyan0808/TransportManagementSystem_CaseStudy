using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transport_Management_System_App.Models;
using Transport_Management_System_App.myexceptions;
using Transport_Management_System_App.Repository;



namespace Transport_Management_System_App.Service
{
    internal class Vehicle_service
    {
        TransportManagementServiceImpl repo = new TransportManagementServiceImpl();
        public void addVehicle()
        {
            try
            {
                Console.WriteLine("Enter model : ");
                String Model = Console.ReadLine();

                Console.WriteLine("Enter capacity : ");
                int Capacity = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter Type : ");
                String Type = Console.ReadLine();

                Console.WriteLine("Enter status : ");
                String Status = Console.ReadLine();
                
                Vehicles vehicles = new Vehicles(Model,Capacity,Type,Status);

                repo.addVehicle(vehicles);

                Console.WriteLine("Vehicle added successfully...");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        public void updateVehicle()
        {
            try
            {
                Console.WriteLine("Enter Vechicle ID to update : ");
                int id = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter model : ");
                string model = Console.ReadLine();

                Console.WriteLine("Enter capacity : ");
                int capacity = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter Type : ");
                string type = Console.ReadLine();

                Console.WriteLine("Enter status : ");
                string status = Console.ReadLine();

                Vehicles vehicle = new Vehicles(model, capacity, type, status);
                try
                {
                    repo.updateVehicle(id, vehicle);
                    Console.WriteLine("Updated vehicle successfully !");
     
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void deleteVehicle()
        {
            try
            {
                Console.WriteLine("Enter Vehicle ID : ");
                int vehicleId = int.Parse(Console.ReadLine());

                if (repo.deleteVehicle(vehicleId))
                {
                    Console.WriteLine("Deleted successfully !");
                }
                else
                {
                    Console.WriteLine("Vehicle not exist...");
                }
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine("Try with valid vehicle ID");
            }
        }

        public void scheduleTrip()
        {
            Console.WriteLine("Enter Vehicle ID : ");
            int vehicleId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Route ID : ");
            int routeId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Departure Date : ");
            String  departureDate = Console.ReadLine();
            Console.WriteLine("Enter Arrival Date : ");
            string arrivalDate = Console.ReadLine();
            try
            {
                if (repo.scheduleTrip(vehicleId, routeId, departureDate, arrivalDate))
                    Console.WriteLine("Trip scheduled successfully...");
                else
                    Console.WriteLine("Trip schedule failed...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return;
        }


        public void cancelTrip()
        {
            Console.WriteLine("Enter Trip Id : ");
            int tripId = int.Parse(Console.ReadLine());

            try
            {
                if(repo.cancelTrip(tripId))
                {
                    Console.WriteLine("Trip Cancelled !");
                    return;
                }
                else
                {
                    Console.WriteLine("Trip Do Not Exist !");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void bookTrip()
        {
            Console.WriteLine("Enter Trip Id : ");
            int tripId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Passenger ID : ");
            int passengerId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Booking Date");
            String bookingDate = Console.ReadLine();


            try
            {
                if( repo.bookTrip(tripId, passengerId, bookingDate))
                {
                    Console.WriteLine("Trip Booked Successfully !");
                    return;
                }
            }
            catch( Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }



        public void cancelBooking()
        {
            Console.WriteLine("Enter Booking ID : ");
            int bookingId = int.Parse(Console.ReadLine());

            try
            {
                repo.cancelBooking(bookingId);
                Console.WriteLine("Cancelled Booking Successfully !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void allocateDriver()
        {
            Console.WriteLine("Enter Trip ID : ");
            int tripId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Driver ID : ");
            int driverId = int.Parse(Console.ReadLine());

            try
            {
                if (repo.allocateDriver(tripId, driverId))
                {
                    Console.WriteLine("Driver Allocated !");
                    return;
                }
                else
                {
                    Console.WriteLine("Driver Allocation Failed !");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void getBookingsByPassenger()
        {
            Console.WriteLine("Enter Passenger ID : ");
            int passengerId = int.Parse(Console.ReadLine());

            try
            {
                List<Bookings> bookings = repo.getBookingByPassenger(passengerId);
                foreach(Bookings detail in bookings)
                {
                    Console.WriteLine(detail.ToString());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void getBookingsByTrip()
        {
            Console.WriteLine("Enter Trip ID : ");
            int tripId = int.Parse(Console.ReadLine());

            try
            {
                List<Bookings> bookings = repo.getBookingsByTrip(tripId);
                foreach(Bookings item in bookings)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void getAvailableDrivers()
        {
            try
            {
                List<Driver> driver_list = repo.getAvailableDrivers();
                foreach(Driver driver in driver_list)
                {
                    Console.WriteLine(driver.ToString());
                }
            }
            catch( Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void deallocateDriver()
        {
            try
            {
                Console.WriteLine("Enter Trip ID : ");
                int tripId = int.Parse(Console.ReadLine()); 
                if(repo.deallocateDriver(tripId))
                {
                    Console.WriteLine("Driver Deallocated Successfully !");
                    return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
