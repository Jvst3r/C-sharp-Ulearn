using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Names
{
    // зде
    internal class CreativityTask
    {
        public static HistogramData GetBirthsPerYearHistogram(NameData[] names, string name)
        {
            //данные в тексте даны за XX век, то есть с 1900 по 1999 (2000 год исключен, хотя это тоже еще XX век)
            var birthdayCountPerYear = new double[100]; //массив счета дней рождений по годам ( 0 элемент-1900 год, 99 элемент - 1999 год)
            var years = new string[100];
            for (int i = 0;i < years.Length;i++) //заполняем массив years годами XX века
                years[i] = (1900 + i).ToString();
           
            foreach (var nameOfHuman in names)
            {
                if (nameOfHuman.Name == name) //считаем дни рождения
                    birthdayCountPerYear[nameOfHuman.BirthDate.Year - 1900] += 1;
            }
            if (name == "михаил")
                return new HistogramData(string.Format("Рождаемость людей с именем " + name + " по годам \n " +
                    "(для просмотра года " + "наведите курсор на график и зажмите левую кнопку мыши) \n" +
                "1945 - Михаил возвдвиг флаг над Рейхстагом(тренд на повышение) \n" +
                "1985-1991 - Правление М.Горбачева и развал СССР (резкий тренд на понижение)"), years, birthdayCountPerYear);
            if (name == "юрий")
                return new HistogramData(string.Format("Рождаемость людей с именем " + name + " по годам \n " +
                    "(для просмотра года наведите курсор на график и зажмите левую кнопку мыши) \n" +
                "1961 - Юрий Гагарин летит в космос (рождаемость детей с его именем тоже)"),years, birthdayCountPerYear);
            if (name == "борис")
                return new HistogramData(string.Format("Рождаемость людей с именем " + name + " по годам \n " +
                    "(для просмотра года наведите курсор на график и зажмите левую кнопку мыши) \n" +
                "90e - выпускник УПИ(ныне УрФУ) уничтожил своей политикой не только экономику, \n" +
                "но и рождаемость детей с его именем :)"), years, birthdayCountPerYear);
            if (name == "владимир")
                return new HistogramData(string.Format("Рождаемость людей с именем " + name + " по годам \n " +
                    "(для просмотра года наведите курсор на график и зажмите левую кнопку мыши) \n" +
                "1952 - год рождения В.В.Путина (50e-60e пик рождаемости с этим именем) \n" +
                "честно, я пытался найти закономерность, но не смог \n" +
                "ну разве что имя Владимир стало популярно из-за анекдотов про 'Вовочек' :)"), years, birthdayCountPerYear);
            if (name == "арсений")
                return new HistogramData(string.Format("Рождаемость людей с именем " + name + " по годам \n " +
                    "(для просмотра года наведите курсор на график и зажмите левую кнопку мыши) \n" +
                "Ну ради интереса я посмотрел моё имя,сейчас Арсениев гораздо больше стало ((( "), years, birthdayCountPerYear);

            return new HistogramData(string.Format("Рождаемость людей с именем " + name + " по годам \n " +
                    "(для просмотра года " + "наведите курсор на график и зажмите левую кнопку мыши)"),years, birthdayCountPerYear);
        }
    }
}
