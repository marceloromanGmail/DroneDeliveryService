using DroneDeliveryService.Models;

namespace DroneDeliveryService.Services
{
    public class DeliveryService
    {
        public void LoadTripsToDrones(List<Drone> drones, List<Location> locations)
        {
            var groupedLocations = new List<List<Location>>();
            var remainingValues = new List<Location>(locations);
            remainingValues.Sort((x, y) => y.PackageWeight.CompareTo(x.PackageWeight));

            var groupSets = CreateGroupSetsLocations(groupedLocations);

            // Assign remaining values to groups
            foreach (var location in remainingValues)
            {
                var addedToExistingGroup = AssignLocationToExistingGroup(location, groupedLocations, groupSets, drones);

                // If the location was not added to any existing group, create a new group for it
                if (!addedToExistingGroup)
                {
                    CreateNewGroupForLocation(location, groupedLocations, groupSets);
                }
            }

            // Load the trips for each drone with the grouped locations
            LoadTrips(drones, groupedLocations);
        }

        private List<HashSet<string>> CreateGroupSetsLocations(List<List<Location>> groupedLocations)
        {
            var groupSets = new List<HashSet<string>>();

            // Iterate over each group and create a set of location names
            foreach (var group in groupedLocations)
            {
                var groupSet = new HashSet<string>(group.Select(x => x.Name));
                groupSets.Add(groupSet);
            }

            return groupSets;
        }

        private bool AssignLocationToExistingGroup(
            Location location, 
            List<List<Location>> groupedLocations, 
            List<HashSet<string>> groupSets, 
            List<Drone> drones)
        {
            var locationWeight = location.PackageWeight;

            // Check if the location can be added to any existing group
            for (int i = 0; i < groupedLocations.Count; i++)
            {
                var group = groupedLocations[i];
                var groupSet = groupSets[i];

                // Check if the location is not already in the group
                if (!groupSet.Contains(location.Name))
                {
                    var addedToExistingGroup = AddLocationToGroup(location, group, groupSet, drones, locationWeight);

                    // If the location was added to an existing group, exit the loop
                    if (addedToExistingGroup)
                        return true;
                }
            }

            return false;
        }

        private bool AddLocationToGroup(
            Location location, 
            List<Location> group, 
            HashSet<string> groupSet, 
            List<Drone> drones, 
            int locationWeight)
        {
            var groupSum = group.Sum(x => x.PackageWeight);

            // Check if adding the location to the group exceeds the drone's weight limit
            foreach (var drone in drones)
            {
                if (groupSum + locationWeight <= drone.MaxWeight)
                {
                    // Add the location to the group and update the group set
                    group.Add(location);
                    groupSet.Add(location.Name);
                    return true;
                }
            }

            return false;
        }

        private void CreateNewGroupForLocation(
            Location location, 
            List<List<Location>> groupedLocations, 
            List<HashSet<string>> groupSets)
        {
            groupedLocations.Add(new List<Location> { location });
            groupSets.Add(new HashSet<string> { location.Name });
        }

        private void LoadTrips(List<Drone> drones, List<List<Location>> groupedLocations)
        {
            // Assign groups of locations to drones in a round-robin manner
            for (int i = 0; i < groupedLocations.Count; i++)
            {
                var group = groupedLocations[i];
                var drone = drones[i % drones.Count];
                drone.Trips.Add(group);
            }
        }
    }
}
