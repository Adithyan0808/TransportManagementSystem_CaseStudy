using BikeModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeModule.Data
{
    internal interface IBikeRepository
    {
        void acceptBikeDetails(Bike bike);
        void displayBikeDetails();
    }
}
