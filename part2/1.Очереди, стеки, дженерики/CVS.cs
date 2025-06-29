using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using static Clones.CloneVersionSystem;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
    internal static List<Clone> clones = new List<Clone>();

    public CloneVersionSystem() => clones = new List<Clone> { new Clone() };
    enum Commands //список команд
    {
        Learn,
        Relearn,
        Rollback,
        Clone,
        Check
    }

    internal class SingleConnectedStack<T> //до меня доперло, что нужен именно класс элемента, а не стек:)
    {
        public T Value; //значение
        public SingleConnectedStack<T> ParentElement; //ссылка на предыдущий объект в "стеке"

        public SingleConnectedStack(SingleConnectedStack<T> parent, T value)
        {
            this.ParentElement = parent;
            this.Value = value;
        }
    }

    internal class Clone
    {
        public int Number;
        public SingleConnectedStack<int> Educations; //программы, которые клон освоил
        internal SingleConnectedStack<int> Rollbacks; //список откатов

        public Clone()
        {
            Number = 1;
        }

        public Clone(Clone parent) //конструктор клона от клона
        {
            this.Number = clones.Count;
            this.Educations = parent.Educations;
            this.Rollbacks = parent.Rollbacks;
        }

        public void Learn(int numberOfEducation) //обучаем клона программе
        {
            this.Rollbacks = null; //как оказалось нужно обнулять историю откатов
            this.Educations = new SingleConnectedStack<int>(Educations, numberOfEducation); //добавляем программу в стек
        }

        public void Relearn()
        {
            if (Rollbacks != null) //если в стеке есть откаты
            {
                this.Educations = new SingleConnectedStack<int>(Educations, Rollbacks.Value); //возвращаем
                this.Rollbacks = Rollbacks.ParentElement; // возвращаемся на одну ступень назад
            }
            else throw new Exception("Нечего переучивать!");
        }

        public void Rollback() //откатываем обучение
        {
            if (Educations != null) //если есть образование, то откатываем
            {
                this.Rollbacks = new SingleConnectedStack<int>(Rollbacks, Educations.Value);//заносим в стек откатов
                this.Educations = Educations.ParentElement; //удаляем из знаний клона 
            }
            else throw new Exception("Нечего откатывать, клон обладает только базовыми знаниями!");
        }
    }

    public string Execute(string query)
    {
        var command = query.Split(' ')[0];
        var number = int.Parse(query.Split(' ')[1]);
        var clone = clones[number - 1];
        return ChooseCommand(command, clone, query);
        throw new Exception("Программа еще разрабатывается...");
    }

    internal static string ChooseCommand(string command, Clone clone, string query)
    {
        switch (command)
        {
            case "learn":
                var programm = int.Parse(query.Split(' ')[2]);
                clone.Learn(programm);
                return null;
            case "relearn":
                clone.Relearn();
                return null;
            case "clone":
                //clones.Add(new Clone(clones.Count+1, new List<int>(clone.educations), new Stack<int>(clone.rollbacks.values)));
                clones.Add(new Clone(clone));
                return null;
            case "check":
                if (clone.Educations != null)
                    return clone.Educations.Value.ToString();                       //очень сложно написано, но
                return "basic";                                                     //тут возвращается последняя
                                                                                    //освоенная программа
            case "rollback":
                clone.Rollback();
                return null;
            default:
                throw new Exception("Некорректная команда. Проверьте правильность ввода!");
        }
    }
}