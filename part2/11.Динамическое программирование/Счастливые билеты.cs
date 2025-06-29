using System.Numerics;

namespace Tickets;

public static class TicketsTask
{
    public static BigInteger Solve(int halfLen, int totalSum)
    {
        // Если общая сумма нечётная, то счастливых билетов быть не может 
        if (totalSum % 2 != 0)
            return 0;

        var sumPerHalf = totalSum / 2; // Сумма для каждой половины
        var maxSumPerHalf = 9 * halfLen;


        if (sumPerHalf < 0 || sumPerHalf > maxSumPerHalf)
            return 0;

        var sumCombinations = CalculateSumCombinations(halfLen, maxSumPerHalf);

        // Количество способов для sumPerHalf в каждой половине
        var count = sumCombinations[sumPerHalf];
        return count * count;
    }

    private static BigInteger[] CalculateSumCombinations(int halfLen, int maxSumPerHalf)
    {
        var sumCombinations = new BigInteger[maxSumPerHalf + 1];
        sumCombinations[0] = 1;

        for (var digits = 0; digits < halfLen; digits++)
        {
            var updatedSumCombinations = new BigInteger[maxSumPerHalf + 1];
            for (var currentSum = 0; currentSum <= maxSumPerHalf; currentSum++)
            {
                if (sumCombinations[currentSum] == 0) continue;
                for (var nextDigit = 0; nextDigit <= 9; nextDigit++)
                {
                    var newSum = currentSum + nextDigit;
                    if (newSum > maxSumPerHalf) continue;
                    updatedSumCombinations[newSum] += sumCombinations[currentSum];
                }
            }
            sumCombinations = updatedSumCombinations;
        }

        return sumCombinations;
    }
}
