using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_Management_System_App.Models
{
    internal class Routes
    {

        private int routeID;
        private String startDestination;
        private String endDestination;
        private String distance;

        public Routes(int routeID, string startDestination, string endDestination, string distance)
        {
            this.RouteID = routeID;
            this.StartDestination = startDestination;
            this.EndDestination = endDestination;
            this.Distance = distance;
        }

        public int RouteID { get => routeID; set => routeID = value; }
        public string StartDestination { get => startDestination; set => startDestination = value; }
        public string EndDestination { get => endDestination; set => endDestination = value; }
        public string Distance { get => distance; set => distance = value; }




    }
}
