using DroneDeliveryService.Models;

namespace DroneDeliveryService.Helpers
{
    public static class ReadInputFilesService
    {
        public static readonly string PathBase = $"{AppDomain.CurrentDomain.BaseDirectory}\\InputFiles\\";

        public static void ReadFile(string fileName, out List<Drone> drones, out List<Location> locations)
        {
            drones = new List<Drone>();
            locations = new List<Location>();

            var fileFullPath = $"{PathBase}\\{fileName}";
            if (!File.Exists(fileFullPath)) return;

            var inputLines = File.ReadAllLines(fileFullPath).ToList();
            drones = GetDronesFromData(inputLines.FirstOrDefault());
            locations = GetLocationsFromData(inputLines);
        }

        private static List<Drone> GetDronesFromData(string? inputLines)
        {
            var drones = new List<Drone>();
            if (inputLines == null) return drones;

            var droneInfo = inputLines.Split(',');
            for (int i = 0; i < droneInfo.Length; i += 2)
            {
                var droneName = droneInfo[i].Trim();
                var maxWeight = int.Parse(droneInfo[i + 1].Trim(' ', '[', ']'));
                drones.Add(new Drone
                {
                    Name = droneName,
                    MaxWeight = maxWeight
                });
            }
            return drones;
        }

        private static List<Location> GetLocationsFromData(List<string> inputLines)
        {
            var locations = new List<Location>();
            if (!inputLines.Any()) return locations;

            inputLines.RemoveAt(0);
            var data = string.Join(string.Empty, inputLines).Replace(" ", string.Empty).Replace(",", string.Empty);
            var locationsInfo = data.Substring(1, data.Length - 2).Split("][");
            for (int i = 0; i < locationsInfo.Length; i += 2)
            {
                var locationName = $"[{locationsInfo[i]}]";
                var packageWeight = int.Parse(locationsInfo[i + 1]);
                locations.Add(new Location
                {
                    Name = locationName,
                    PackageWeight = packageWeight
                });
            }
            return locations;
        }
    }
}
