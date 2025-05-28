using System;
using System.Drawing;

namespace RoutePlanning;

public static class PathFinderTask
{
    public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
    {
        var size = checkpoints.Length; //кол-во чекпоинтов
        var bestCost = double.MaxValue; //лучшая стоимость перемещения
        var bestOrder = new int[size]; // массив с порядком перемещений
                                       //bestOrder = 
        MakeTrivialPermutation(new int[size], bestOrder, 1, 0, ref bestCost, checkpoints);
        return bestOrder;
    }

    private static void MakeTrivialPermutation(int[] permutation,
            int[] bestPermutation,
            int position,
            double currentCost,
            ref double bestCost,
            Point[] checkpoints)
    {
        if (currentCost >= bestCost) //если стоимость УЖЕ получается больше, чем есть на данный момент
            return;
        if (position == permutation.Length) //если последняя позиция
        {
            permutation.CopyTo(bestPermutation, 0);
            bestCost = currentCost;
            return;
        }
        for (int i = 0; i < permutation.Length; i++)
        {
            if (Array.IndexOf(permutation, i, 0, position) != -1)
                continue;
            permutation[position] = i;
            MakeTrivialPermutation( //вызываем эту же рекурсивную функцию
                permutation, bestPermutation, position + 1,// увеличиваем позицию
                currentCost + PointExtensions.DistanceTo(checkpoints[permutation[position]],
                checkpoints[permutation[position - 1]]),
                ref bestCost,
                checkpoints);
        }
    }
}