using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    // Класс для хранения данных о пути: предыдущая точка и общая стоимость
    public class DijkstraNodeData
    {
        public Point Previous { get; set; } // Предыдущая точка в пути
        public int TotalCost { get; set; }  // Общая стоимость пути до этой точки
    }

    public class DijkstraPathFinder
    {
        // Основной метод для поиска путей
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
        {
            var (remainingTargets, openNodes, visitedNodes, nodeData) = InitializeDijkstra(start, targets);

            // Пока есть необработанные цели
            while (remainingTargets.Count > 0)
            {
                // Выбираем следующую точку с минимальной стоимостью
                var nextNode = GetNextNodeWithMinimalCost(nodeData, openNodes);

                // Если точек для обработки нет — завершаем
                if (nextNode.NextPoint == new Point(-1, -1)) yield break;

                // Обновляем данные соседей выбранной точки
                UpdateNeighbors(nextNode.NextPoint, state, visitedNodes, openNodes, nodeData);

                // Собираем и возвращаем найденные пути
                foreach (var path in BuildPathToTarget(remainingTargets, nextNode.NextPoint, nodeData))
                {
                    yield return path;
                }
            }
        }

        private (HashSet<Point>, HashSet<Point>, HashSet<Point>, Dictionary<Point, DijkstraNodeData>)
            InitializeDijkstra(Point start, IEnumerable<Point> targets)
        {
            var remainingTargets = new HashSet<Point>(targets);
            var openNodes = new HashSet<Point> { start }; // Точки, которые нужно обработать
            var visitedNodes = new HashSet<Point>();      // Уже обработанные точки

            var nodeData = new Dictionary<Point, DijkstraNodeData>
            {
                [start] = new DijkstraNodeData
                {
                    Previous = new Point(-1, -1), // Специальное значение для стартовой точки
                    TotalCost = 0
                }
            };

            return (remainingTargets, openNodes, visitedNodes, nodeData);
        }

        // Обновляем данные для соседних точек текущей точки
        // Основной метод обновления соседей
        private static void UpdateNeighbors(
            Point currentPoint,
            State state,
            HashSet<Point> visitedNodes,
            HashSet<Point> openNodes,
            IDictionary<Point, DijkstraNodeData> nodeData)
        {
            // Возможные направления движения: вправо, вниз, влево, вверх
            var directions = new[] { (1, 0), (0, 1), (-1, 0), (0, -1) };

            foreach (var (dx, dy) in directions)
            {
                ProcessSingleDirection(currentPoint, state, visitedNodes, openNodes, nodeData, dx, dy);
            }

            FinalizeCurrentNodeProcessing(currentPoint, openNodes, visitedNodes);
        }

        // Обработка одного направления движения
        private static void ProcessSingleDirection(
            Point currentPoint,
            State state,
            HashSet<Point> visitedNodes,
            HashSet<Point> openNodes,
            IDictionary<Point, DijkstraNodeData> nodeData,
            int dx,
            int dy)
        {
            // Вычисляем координаты соседней точки
            var neighbor = new Point(currentPoint.X + dx, currentPoint.Y + dy);

            // Проверяем, что точка внутри карты, не стена и не посещена
            if (state.InsideMap(neighbor) && !state.IsWallAt(neighbor) && !visitedNodes.Contains(neighbor))
            {
                // Добавляем соседа в список для обработки
                openNodes.Add(neighbor);

                // Рассчитываем новую стоимость пути до соседа
                var newCost = nodeData[currentPoint].TotalCost + state.CellCost[neighbor.X, neighbor.Y];

                // Если новая стоимость меньше текущей, обновляем данные
                if (!nodeData.ContainsKey(neighbor) || nodeData[neighbor].TotalCost > newCost)
                {
                    nodeData[neighbor] = new DijkstraNodeData
                    {
                        Previous = currentPoint,
                        TotalCost = newCost
                    };
                }
            }
        }

        // Завершение обработки текущей точки
        private static void FinalizeCurrentNodeProcessing(
            Point currentPoint,
            HashSet<Point> openNodes,
            HashSet<Point> visitedNodes)
        {
            // Помечаем текущую точку как обработанную
            openNodes.Remove(currentPoint);
            visitedNodes.Add(currentPoint);
        }

        // Собираем путь до цели и возвращаем его
        private IEnumerable<PathWithCost> BuildPathToTarget(
            HashSet<Point> targets,
            Point currentPoint,
            Dictionary<Point, DijkstraNodeData> nodeData)
        {
            // Если текущая точка не является целью, завершаем
            if (!targets.Contains(currentPoint)) yield break;

            // Удаляем цель из списка целей
            targets.Remove(currentPoint);

            // Собираем точки пути в обратном порядке (от цели к старту)
            var pathPoints = new List<Point>();
            for (var point = currentPoint; point != new Point(-1, -1); point = nodeData[point].Previous)
            {
                pathPoints.Add(point);
            }

            // Переворачиваем путь, чтобы он шёл от старта к цели
            pathPoints.Reverse();

            // Возвращаем путь и его стоимость
            yield return new PathWithCost(nodeData[currentPoint].TotalCost, pathPoints.ToArray());
        }

        // Находим следующую точку с минимальной стоимостью для обработки
        private (Point NextPoint, double MinimalCost) GetNextNodeWithMinimalCost(
            Dictionary<Point, DijkstraNodeData> nodeData,
            IEnumerable<Point> openNodes)
        {
            var minimalCost = double.PositiveInfinity;
            var nextPoint = new Point(-1, -1); // Специальное значение для "точки не найдено"

            foreach (var point in openNodes)
            {
                if (nodeData.TryGetValue(point, out var data) && data.TotalCost < minimalCost)
                {
                    minimalCost = data.TotalCost;
                    nextPoint = point;
                }
            }

            return (nextPoint, minimalCost);
        }
    }
}