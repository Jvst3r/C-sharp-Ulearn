using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
    {
        var chestsHash = ToChestLocationHashSet(chests);
        return GetPaths(map, start)
            .Where(path => chestsHash.Contains(path.Value));
    }

    private static IEnumerable<SinglyLinkedList<Point>> GetPaths(Map map, Point start)
    {
        var queue = new Queue<SinglyLinkedList<Point>>();
        var visited = new HashSet<Point>();

        queue.Enqueue(new SinglyLinkedList<Point>(start));
        visited.Add(start);

        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();
            foreach (var nextPath in ProcessCurrentPoint(map, currentPath, visited))
            {
                queue.Enqueue(nextPath);
                visited.Add(nextPath.Value);
                yield return nextPath;
            }
        }
    }

    private static IEnumerable<SinglyLinkedList<Point>> ProcessCurrentPoint(
        Map map,
        SinglyLinkedList<Point> currentPath,
        HashSet<Point> visited)
    {
        var point = currentPath.Value;

        if (map.Dungeon[point.X, point.Y] == MapCell.Wall)
            yield break;

        foreach (var direction in Walker.PossibleDirections)
        {
            var nextPoint = point + direction;
            if (DirectionIsRight(map, nextPoint, visited))
            {
                yield return new SinglyLinkedList<Point>(nextPoint, currentPath);
            }
        }
    }

    private static bool DirectionIsRight(Map map, Point nextPoint, HashSet<Point> visited) =>
        (map.InBounds(nextPoint)) && //находимся в пределах карты
        (!visited.Contains(nextPoint)); //еще не посещали точку


    private static HashSet<Point> ToChestLocationHashSet(Chest[] chests) =>
        chests
        .Select(chest => chest.Location)
        .ToHashSet();
}