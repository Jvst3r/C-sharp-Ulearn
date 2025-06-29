using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var startPaths = FindAllPaths(map, map.InitialPosition);
            var exitPaths = FindAllPaths(map, map.Exit);

            var bestChest = map.Chests
                .Where(c => startPaths.ContainsKey(c.Location) && exitPaths.ContainsKey(c.Location))
                .OrderBy(c => startPaths[c.Location].Length + exitPaths[c.Location].Length)
                .ThenByDescending(c => c.Value)
                .FirstOrDefault();

            return ProcessingReturnPath(map, bestChest, startPaths, exitPaths);
        }

        private static MoveDirection[] ProcessingReturnPath(Map map, Chest? bestChest,
            Dictionary<Point, SinglyLinkedList<Point>> startPaths,
            Dictionary<Point, SinglyLinkedList<Point>> exitPaths)
        {
            if (bestChest != null)
            {
                var toChest = GetPathPoints(startPaths[bestChest.Location]);
                var fromChest = GetPathPoints(exitPaths[bestChest.Location]);
                fromChest.Reverse();
                var fullPath = toChest.Concat(fromChest.Skip(1)).ToList().ToList();
                return ConvertToDirections(fullPath);
            }

            if (startPaths.ContainsKey(map.Exit))
            {
                var toExit = GetPathPoints(startPaths[map.Exit]);
                return ConvertToDirections(toExit);
            }

            return new MoveDirection[0];
        }

        private static List<Point> GetPathPoints(SinglyLinkedList<Point> path)
        {
            var points = new List<Point>();
            while (path != null)
            {
                points.Add(path.Value);
                path = path.Previous;
            }
            points.Reverse();
            return points;
        }

        private static MoveDirection[] ConvertToDirections(List<Point> path)
        {
            var directions = new List<MoveDirection>();
            for (int i = 0; i < path.Count - 1; i++)
            {
                var offset = path[i + 1] - path[i];
                directions.Add(Walker.ConvertOffsetToDirection(offset));
            }
            return directions.ToArray();
        }

        private static Dictionary<Point, SinglyLinkedList<Point>> FindAllPaths(Map map, Point start)
        {
            var queue = new Queue<SinglyLinkedList<Point>>();
            var visited = new Dictionary<Point, SinglyLinkedList<Point>>();
            var startNode = new SinglyLinkedList<Point>(start);

            queue.Enqueue(startNode);
            visited[start] = startNode;

            ProcessBfsQueue(queue, visited, map);
            return visited;
        }

        private static void ProcessBfsQueue(Queue<SinglyLinkedList<Point>> queue,
            Dictionary<Point, SinglyLinkedList<Point>> visited,
            Map map)
        {
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                ExploreNeighbors(current, queue, visited, map);
            }
        }

        private static void ExploreNeighbors(
            SinglyLinkedList<Point> current,
            Queue<SinglyLinkedList<Point>> queue,
            Dictionary<Point, SinglyLinkedList<Point>> visited,
            Map map)
        {
            foreach (var direction in Walker.PossibleDirections)
            {
                var nextPoint = current.Value + direction;
                if (IsAccessible(nextPoint, map) && !visited.ContainsKey(nextPoint))
                {
                    var nextNode = new SinglyLinkedList<Point>(nextPoint, current);
                    visited[nextPoint] = nextNode;
                    queue.Enqueue(nextNode);
                }
            }
        }

        private static bool IsAccessible(Point point, Map map) => map.InBounds(point) &&
                                        map.Dungeon[point.X, point.Y] != MapCell.Wall;
    }
}