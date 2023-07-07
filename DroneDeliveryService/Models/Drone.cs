namespace DroneDeliveryService.Models
{
    public class Drone
    {
        public string Name { get; set; }
        public int MaxWeight { get; set; }

        public List<List<Location>> Trips { get; set; }

        public Drone() => Trips = new List<List<Location>>();
    }
}
