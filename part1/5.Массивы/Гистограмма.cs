using System;

namespace Names;

internal static class HistogramTask
{
    public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
    {
        var days = new string[31]; //массив с количеством дней в месяце
        var birthdayCountPerDay = new double[31]; //массив с количеством дней рождений по дням
        for (int i = 0; i < days.Length; i++)
            days[i] = (i + 1).ToString();

        foreach (var nameOfHuman in names)
        {
            if (nameOfHuman.Name == name) //считаем дни рождения
                birthdayCountPerDay[nameOfHuman.BirthDate.Day - 1] += 1;
        }

        birthdayCountPerDay[0] = 0.0; //по ТЗ данные с первого числа не учитываются
        return new HistogramData(string.Format("Рождаемость людей с именем " + name), days, birthdayCountPerDay); //делаем гистограмму насколько я понял
    }
}