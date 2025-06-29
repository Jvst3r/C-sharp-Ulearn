using System;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;

namespace hashes;

public class GhostsTask :
    IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
    IMagic
{
    private readonly static byte[] documentArr = { 1, 1, 1, 1, 1 };
    private Document document = new Document("Неломаемый", Encoding.UTF7, documentArr);
    private Vector vector = new Vector(10, 20);
    private Segment segment = new Segment(new Vector(20, 30), new Vector(30, 40));
    private Cat cat = new Cat("Глаша", "дворовая", DateTime.Now);
    private Robot robot = new Robot("R2-D2");
    public void DoMagic()
    {
        //document.Title = "Всё таки ломаемый";
        //documentArr = new byte[] { 1, 1, 1 , 0};
        documentArr[0]++;
        //document = null;
        vector.Add(new Vector(10, 1000)); //сломано
        segment.Start.Add(vector); //сломано
        cat.Rename("Глафира"); //сломано
                               //robot.Move(10, 10000);
                               //robot.Battery = 10;
        Robot.BatteryCapacity++;
    }

    // Чтобы класс одновременно реализовывал интерфейсы IFactory<A> и IFactory<B> 
    // придется воспользоваться так называемой явной реализацией интерфейса.
    // Чтобы отличать методы создания A и B у каждого метода Create нужно явно указать, к какому интерфейсу он относится.
    // На самом деле такое вы уже видели, когда реализовывали IEnumerable<T>.

    Document IFactory<Document>.Create() => document;

    Vector IFactory<Vector>.Create() => vector;

    Segment IFactory<Segment>.Create() => segment;

    Cat IFactory<Cat>.Create() => cat;

    Robot IFactory<Robot>.Create() => robot;
}