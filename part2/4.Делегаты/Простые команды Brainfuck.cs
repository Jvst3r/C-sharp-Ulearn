using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        private static readonly string asciiSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            RegisterCoreCommands(vm, read, write);
            RegisterAsciiSymbols(vm);
        }

        private static void RegisterCoreCommands(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            // Основные команды
            vm.RegisterCommand('.', b => { write((char)b.Memory[b.MemoryPointer]); });
            vm.RegisterCommand('+', b => { unchecked { vm.Memory[vm.MemoryPointer]++; } });
            vm.RegisterCommand('-', b => { unchecked { vm.Memory[vm.MemoryPointer]--; } });
            vm.RegisterCommand('>', b => b.MemoryPointer = (b.MemoryPointer == b.Memory.Length - 1) ? 0 : b.MemoryPointer + 1);
            vm.RegisterCommand('<', b => b.MemoryPointer = (b.MemoryPointer == 0) ? b.Memory.Length - 1 : b.MemoryPointer - 1);
            vm.RegisterCommand(',', b => { vm.Memory[vm.MemoryPointer] = (byte)read(); });
        }

        private static void RegisterAsciiSymbols(IVirtualMachine vm)
        {
            for (int i = 0; i < asciiSymbols.Length; i++)
            {
                char symbol = asciiSymbols[i];
                vm.RegisterCommand(symbol, b => vm.Memory[vm.MemoryPointer] = (byte)symbol);
            }
        }
    }
}