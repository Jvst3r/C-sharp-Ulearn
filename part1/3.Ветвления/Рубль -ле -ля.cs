namespace Pluralize;

public static class PluralizeTask
{
    public static string PluralizeRubles(int count)
    {
        if ((count % 10 == 0 || count % 10 > 4) || (count % 100 > 10 && count % 100 < 15)) return "рублей";
        else if (count % 10 == 1) return "рубль";
        else if (count % 10 > 1 && count % 10 <= 4) return "рубля";
        else return "пора устроиться на работу и закрыть кредит"; //для значений меньше нуля
                                                                  // вот такая рофлострочка
    }
}

