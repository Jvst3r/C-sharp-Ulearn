using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Threading.Tasks;
namespace rocket_bot;

public partial class Bot
{
    public Rocket GetNextMove(Rocket rocket)
    {
        var tasks = CreateTasks(rocket); //запускаем поиск параллельно
        var results = Task.WhenAll(tasks).GetAwaiter().GetResult(); //получаем результаты ВСЕХ поисков
        var bestResult = results.OrderByDescending(r => r.Score).First(); //выбираем лучший
        return rocket.Move(bestResult.Turn, level);
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        var tasks = new List<Task<(Turn, double)>>();

        //распределяем итерации между потоками 
        int iterationsPerThread = iterationsCount / threadsCount;
        int remainder = iterationsCount % threadsCount; //остаток для первых потоков 

        for (int i = 0; i < threadsCount; i++)
        {
            //задаем итерации на поток с учётом остатка
            var currentIterations = iterationsPerThread;
            if (i < remainder)
                currentIterations = +1;

            //для каждого потока свой Random 
            var threadSeed = random.Next();
            var threadRandom = new Random(threadSeed);

            //ищем
            tasks.Add(Task.Run(() =>
                SearchBestMove(rocket, threadRandom, currentIterations)));
        }

        return tasks;
    }
}


