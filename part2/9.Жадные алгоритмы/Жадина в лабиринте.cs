using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            if (state.Chests.Count < state.Goal)
                return new List<Point>();

            var collectedChestsPath = new List<Point>();
            var pathFinder = new DijkstraPathFinder();

            while (state.Scores < state.Goal)
            {
                // Ищем путь к ближайшему сундуку
                var nearestChestPath = pathFinder.GetPathsByDijkstra(
                    state,
                    state.Position,
                    state.Chests
                ).FirstOrDefault();

                if (nearestChestPath == null || state.Energy < nearestChestPath.Cost)
                    return new List<Point>();

                UpdateGameState(state, nearestChestPath);
                UpdatePath(collectedChestsPath, nearestChestPath);
            }

            return collectedChestsPath;
        }

        private void UpdateGameState(State state, PathWithCost pathToChest)
        {
            state.Energy -= pathToChest.Cost;
            state.Position = pathToChest.Path.Last();
            state.Chests.Remove(state.Position);
            state.Scores++;
        }

        private void UpdatePath(List<Point> fullPath, PathWithCost pathToChest)
        {
            // Пропускаем стартовую точку (текущую позицию)
            fullPath.AddRange(pathToChest.Path.Skip(1));
        }
    }
}


