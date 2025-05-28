namespace Names;

internal static class HeatmapTask
{
    public static double[,] GetStatisticForHeatMap(NameData[] names, double[,] BirthdayForHeatMap)
    {
        for (int i = 0; i < 30; i++) //считаем по дням
        {
            var oneOfMonths = new double[12];

            foreach (var nameOfHuman in names)
            {
                if (nameOfHuman.BirthDate.Day == i + 2)
                    oneOfMonths[nameOfHuman.BirthDate.Month - 1] += 1; //считаем данные для массива
            }

            for (int j = 0; j < 12; j++)
                BirthdayForHeatMap[i, j] = oneOfMonths[j]; //переносим в массив данные за месяц по дням
        }
        return BirthdayForHeatMap;
    }

    public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
    {
        var days = new string[30];// с 2 по 31 числа - 30 дней
        var months = new string[12]; // 12 месяцев
        var BirthdayForHeatMap = new double[30, 12]; //массив для данных

        for (int i = 0; i < days.Length; i++) //заполняем дни месяца
            days[i] = (i + 2).ToString();
        for (int i = 0; i < months.Length; i++) // заполняем месяцы (по ТЗ числами!)
            months[i] = (i + 1).ToString();

        BirthdayForHeatMap = GetStatisticForHeatMap(names, BirthdayForHeatMap);

        return new HeatmapData("Карта интенсивностей", BirthdayForHeatMap, days, months); // делаем тот самый heatmap
    }
}