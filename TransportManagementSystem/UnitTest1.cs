
using Transport_Management_System_App.Repository;


namespace TransportManagementSystem
{
    
    public class Tests
    {
        TransportManagementServiceImpl repo = new TransportManagementServiceImpl();
        [Test]
        public void TestAllocateDriver()
        {
            int tripId = 1;
            int driverId = 1;
            Assert.IsFalse(repo.allocateDriver(tripId,driverId));
        }

        [Test]
        public void Test()
        {
            Assert.Pass();
        }
    }
}