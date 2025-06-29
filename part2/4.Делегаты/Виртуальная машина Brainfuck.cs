using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        public string Instructions { get; } // "код" на brainfuck
        public int InstructionPointer { get; set; } // указатель на команду в "коде"
        public byte[] Memory { get; } // массив с памятью че сказать
        public int MemoryPointer { get; set; } //указатель на байт в памяти

        private Dictionary<char, Action<IVirtualMachine>> commands;

        public VirtualMachine(string program, int memorySize)
        {
            Instructions = program; // запоминаем наш "код"
            Memory = new byte[memorySize]; //создаем массив с байтами определенного размера
            commands = new Dictionary<char, Action<IVirtualMachine>>(program.Length);
            InstructionPointer = 0;
            MemoryPointer = 0;
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            if (!commands.ContainsKey(symbol)) commands.Add(symbol, execute); //добавляем в список команд
                                                                              //команду, если ее нет
        }

        public void Run()
        {
            for (; InstructionPointer < Instructions.Length; InstructionPointer++) // идем по командам
                                                                                   // (читаем по символу в строке)
            {
                if (commands.ContainsKey(Instructions[InstructionPointer]))//если в списке команд есть такая команда
                    commands[Instructions[InstructionPointer]](this); //то исполняем ее
            }
        }
    }
}