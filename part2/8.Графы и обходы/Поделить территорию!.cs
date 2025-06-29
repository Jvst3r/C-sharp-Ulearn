using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Rivals
{
    public class RivalsTask
    {
        private static readonly Point[] MovementDirections =
        {
            new Point(0, -1), // Вверх
            new Point(0, 1),  // Вниз
            new Point(1, 0),  // Вправо
            new Point(-1, 0)  // Влево
        };

        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var visitedLocations = new Dictionary<Point, OwnedLocation>();
            var queue = InitializeQueue(map, visitedLocations);

            while (queue.Count > 0)
            {
                var currentLocation = queue.Dequeue();
                if (IsChest(map, currentLocation.Location)) continue;

                ProcessNeighbors(map, currentLocation, visitedLocations, queue);
            }

            return SortLocations(visitedLocations.Values);
        }

        private static Queue<(Point Location, int Owner, int Distance)> InitializeQueue(
            Map map, Dictionary<Point, OwnedLocation> visited)
        {
            var queue = new Queue<(Point, int, int)>();
            for (int playerId = 0; playerId < map.Players.Length; playerId++)
            {
                var startPosition = map.Players[playerId];
                if (visited.ContainsKey(startPosition)) continue;

                var initialLocation = new OwnedLocation(playerId, startPosition, 0);
                visited.Add(startPosition, initialLocation);
                queue.Enqueue((startPosition, playerId, 0));
            }
            return queue;
        }

        private static bool IsChest(Map map, Point location) => map.Chests.Contains(location);


        private static void ProcessNeighbors(
            Map map,
            (Point Location, int Owner, int Distance) current,
            Dictionary<Point, OwnedLocation> visited,
            Queue<(Point Location, int Owner, int Distance)> queue)
        {
            foreach (var direction in MovementDirections)
            {
                var neighbor = current.Location + direction;
                if (!IsAccessibleLocation(map, neighbor)) continue;

                var newDistance = current.Distance + 1;
                if (!ShouldUpdateLocation(visited, neighbor, newDistance)) continue;

                UpdateLocation(visited, queue, neighbor, current.Owner, newDistance);
            }
        }

        private static bool IsAccessibleLocation(Map map, Point location) => map.InBounds(location) &&
                                                     map.Maze[location.X, location.Y] != MapCell.Wall;


        private static bool ShouldUpdateLocation(Dictionary<Point, OwnedLocation> visited, Point location,
            int newDistance) => !visited.TryGetValue(location, out var existing) || newDistance < existing.Distance;


        private static void UpdateLocation(
            Dictionary<Point, OwnedLocation> visited,
            Queue<(Point Location, int Owner, int Distance)> queue,
            Point location,
            int owner,
            int distance)
        {
            var newLocation = new OwnedLocation(owner, location, distance);
            visited[location] = newLocation;
            queue.Enqueue((location, owner, distance));
        }

        private static IEnumerable<OwnedLocation> SortLocations(IEnumerable<OwnedLocation> locations)
       => locations.OrderBy(l => l.Distance)
                .ThenBy(l => l.Owner)
                .ThenBy(l => l.Location.X)
                .ThenBy(l => l.Location.Y);
    }
}