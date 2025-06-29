using Avalonia.Controls;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private LinkedList<T> stack = new LinkedList<T>();
    private int lengthOfStack;
    public LimitedSizeStack(int undoLimit)
    {
        lengthOfStack = undoLimit;
        //var stack = new LinkedList<T>();
    }

    public void Push(T item)
    {
        stack.AddLast(item);

        if (stack.Count > lengthOfStack)
            stack.RemoveFirst();
    }

    public T Pop()
    {
        if (this.Count == 0)
            throw new InvalidOperationException("Stack is empty");
        T removedItem = stack.Last.Value;
        stack.RemoveLast();
        return removedItem;
    }

    public int Count => stack.Count;
}