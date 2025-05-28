static double Calculate(string UserInput)
{
    string[] parts = UserInput.Split(' ');// сплит и запись данных, введённых пользователем
    var money = double.Parse(parts[0]); //начальный капитал
    var percent = 1 + ((double.Parse(parts[1])) * 0.01) / 12; //procent - переменная вида (1 + *месячная доходность*) для рассчета по формуле геометрической прогрессии n-ого члена
    var month = int.Parse(parts[2]); //кол-во месяцев вклада
    return (money * Math.Pow(percent, month)); //к концу вклада на счёте вклада останется ...
}