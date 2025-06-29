namespace rocket_bot;

using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
public class Channel<T> where T : class
{
    private List<T> items;

    //при создании нового Channel создается список
    public Channel()
    {
        items = new List<T>();
    }
    /// <summary>
    /// Возвращает элемент по индексу или null, если такого элемента нет.
    /// При присвоении удаляет все элементы после.
    /// Если индекс в точности равен размеру коллекции, работает как Append.
    /// </summary>
    public T this[int index]
    {
        get
        {
            lock (this)
            {
                return (index >= 0 && index < items.Count) ? items[index] : null;
            }
        }
        set
        {
            lock (this)
            {
                if (index == items.Count)
                {
                    items.Add(value);
                }
                else
                {
                    // Заменяем элемент и удаляем хвост
                    items[index] = value;
                    items.RemoveRange(index + 1, items.Count - index - 1);
                }
            }
        }
    }

    /// <summary>
    /// Возвращает последний элемент или null, если такого элемента нет
    /// </summary>
    public T LastItem()
    {
        lock (this)
        {
            return items.LastOrDefault();
        }
    }

    /// <summary>
    /// Добавляет item в конец только если lastItem является последним элементом
    /// </summary>
    public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
    {
        lock (this)
        {
            //класс - ссылочный тип, метод ReferenceEquals для сравнения ссылочных типов
            if (ReferenceEquals(items.LastOrDefault(), knownLastItem))
            {
                items.Add(item);
            }
        }
    }

    /// <summary>
    /// Возвращает количество элементов в коллекции
    /// </summary>
    public int Count //возможно стоит добавить Lock
    {
        get
        {
            lock (this)
            {
                return items.Count;
            }
        }
    }
}