using DroneDeliveryService.Models;
using DroneDeliveryService.Services;

[TestFixture]
public class DroneManagerTests
{
    [Test]
    public void LoadTripsToDrones_AddsLocationsToDrones_Ok()
    {
        // Arrange
        var drones = new List<Drone>
        {
            new Drone { Name = "[DroneA]", MaxWeight = 200 },
            new Drone { Name = "[DroneB]", MaxWeight = 250 },
            new Drone { Name = "[DroneC]", MaxWeight = 100 }
        };

        var locations = new List<Location>
        {
            new Location { Name = "[LocationA]", PackageWeight = 200 },
            new Location { Name = "[LocationB]", PackageWeight = 150 },
            new Location { Name = "[LocationC]", PackageWeight = 50 },
            new Location { Name = "[LocationD]", PackageWeight = 150 },
            new Location { Name = "[LocationE]", PackageWeight = 100 },
            new Location { Name = "[LocationF]", PackageWeight = 200 },
            new Location { Name = "[LocationG]", PackageWeight = 50 },
            new Location { Name = "[LocationH]", PackageWeight = 80 },
            new Location { Name = "[LocationI]", PackageWeight = 70 },
            new Location { Name = "[LocationJ]", PackageWeight = 50 },
            new Location { Name = "[LocationK]", PackageWeight = 30 },
            new Location { Name = "[LocationL]", PackageWeight = 20 },
            new Location { Name = "[LocationM]", PackageWeight = 50 },
            new Location { Name = "[LocationN]", PackageWeight = 30 },
            new Location { Name = "[LocationO]", PackageWeight = 20 },
            new Location { Name = "[LocationP]",PackageWeight =  90 }
        };

        var sut = new DeliveryService();

        // Act
        sut.LoadTripsToDrones(drones, locations);

        // Assert
        Assert.That(drones.Count, Is.EqualTo(3));
        Assert.That(drones.SelectMany(d => d.Trips).Count(), Is.EqualTo(6));
        Assert.That(drones.SelectMany(d => d.Trips.SelectMany(t => t)).Count, Is.EqualTo(locations.Count));
        Assert.That(drones[0].Trips.Count, Is.EqualTo(2));
        Assert.That(drones[1].Trips.Count, Is.EqualTo(2));
        Assert.That(drones[2].Trips.Count, Is.EqualTo(2));
    }

    [Test]
    public void LoadTripsToDrones_NoData()
    {
        // Arrange
        var drones = new List<Drone>();
        var locations = new List<Location>();

        var sut = new DeliveryService();

        // Act
        sut.LoadTripsToDrones(drones, locations);

        // Assert
        Assert.That(drones.Count, Is.EqualTo(0));
        Assert.That(drones.SelectMany(d => d.Trips).Count(), Is.EqualTo(0));
        Assert.That(drones.SelectMany(d => d.Trips.SelectMany(t => t)).Count, Is.EqualTo(locations.Count));
    }
}
