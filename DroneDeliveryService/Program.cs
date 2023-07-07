using DroneDeliveryService.Helpers;
using DroneDeliveryService.Models;
using DroneDeliveryService.Services;

var fileName = "input.txt";
ReadInputFilesService.ReadFile(fileName, out List<Drone> drones, out List<Location> locations);

new DeliveryService().LoadTripsToDrones(drones, locations);
if (drones.Count == 0 || locations.Count == 0)
{
    Console.WriteLine($"Make sure the input.txt file is into '{ReadInputFilesService.PathBase}\\InputFiles\\{fileName}'");
    return;
}

var output = string.Empty;
foreach (var drone in drones)
{
    output += drone.Name + "\n";
    for (int i = 0; i < drone.Trips.Count; i++)
    {
        output += "Trip #" + (i + 1) + "\n";
        var locationsData = string.Join(", ", drone.Trips[i].Select(t => t.Name).ToList());
        output += locationsData + "\n";
    }
}
Console.WriteLine(output);
Console.WriteLine($"Total Drones: {drones.Count(d => d.Trips.Count > 0)}");
Console.WriteLine($"Total Trips: {drones.SelectMany(d => d.Trips).Count()}");