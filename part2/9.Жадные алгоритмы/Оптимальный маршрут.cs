using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    private List<Point> bestPathSequence = new();
    private int maxCollectedChests;

    public List<Point> FindPathToCompleteGoal(State state)
    {
        var finalPath = new List<Point>();
        var allChestsWithStart = new List<Point>(state.Chests) { state.Position };
        var allPaths = BuildAllPossiblePaths(allChestsWithStart, new DijkstraPathFinder(), state);

        if (allPaths == null || !allPaths.ContainsKey(state.Position))
            return finalPath;

        // Исследуем все пути из стартовой позиции
        ExploreAllPaths(state, allPaths);

        // Собираем итоговый путь из лучшей последовательности
        for (var i = 0; i < bestPathSequence.Count - 1; i++)
        {
            var segment = allPaths[bestPathSequence[i]][bestPathSequence[i + 1]].Path.Skip(1);
            finalPath.AddRange(segment);
        }

        return finalPath;
    }

    private void ExploreAllPaths(State state, Dictionary<Point, Dictionary<Point, PathWithCost>> allPaths)
    {
        foreach (var (targetChest, pathData) in allPaths[state.Position])
        {
            var remainingEnergy = state.Energy - pathData.Cost;
            if (remainingEnergy < 0) continue;

            var currentPath = new List<Point> { state.Position, targetChest };
            ExplorePaths(
                remainingEnergy,
                targetChest,
                state.Chests.Except(new[] { targetChest }).ToList(),
                1,
                currentPath,
                allPaths
            );
        }
    }

    private void ExplorePaths(
        int remainingEnergy,
        Point currentPoint,
        List<Point> availableChests,
        int collectedChests,
        List<Point> currentPath,
        Dictionary<Point, Dictionary<Point, PathWithCost>> pathMap)
    {
        // Обновляем лучший путь, если собрано больше сундуков
        if (collectedChests > maxCollectedChests)
        {
            maxCollectedChests = collectedChests;
            bestPathSequence = new List<Point>(currentPath);
        }

        foreach (var nextChest in availableChests)
        {
            if (!pathMap[currentPoint].TryGetValue(nextChest, out var path) || path.Cost > remainingEnergy)
                continue;

            var newPath = new List<Point>(currentPath) { nextChest };
            ExplorePaths(
                remainingEnergy - path.Cost,
                nextChest,
                availableChests.Except(new[] { nextChest }).ToList(),
                collectedChests + 1,
                newPath,
                pathMap
            );
        }
    }

    private static Dictionary<Point, Dictionary<Point, PathWithCost>> BuildAllPossiblePaths(
        List<Point> points,
        DijkstraPathFinder pathFinder,
        State state)
    {
        var pathDictionary = new Dictionary<Point, Dictionary<Point, PathWithCost>>();

        foreach (var startPoint in points)
        {
            pathDictionary[startPoint] = new Dictionary<Point, PathWithCost>();
            foreach (var path in pathFinder.GetPathsByDijkstra(state, startPoint, state.Chests))
            {
                if (path.Start == path.End) continue; // Пропускаем циклы
                pathDictionary[startPoint][path.End] = path;
            }
        }

        return pathDictionary;
    }
}

