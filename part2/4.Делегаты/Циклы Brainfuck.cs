using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var loops = new Dictionary<int, int>(); // хранит индексы "начало-конец"
            var reversedLoops = new Dictionary<int, int>();
            FindLoops(vm.Instructions, loops, reversedLoops);
            vm.RegisterCommand('[', b =>
            {
                if (b.Memory[b.MemoryPointer] == 0)
                    b.InstructionPointer = loops[b.InstructionPointer];
            });
            vm.RegisterCommand(']', b =>
            {
                if (b.Memory[b.MemoryPointer] != 0)
                    b.InstructionPointer = reversedLoops[b.InstructionPointer];
            });
        }

        private static void FindLoops(string commands, Dictionary<int, int> loops, Dictionary<int, int> reversedLoops)
        {
            var stack = new Stack<int>();
            for (int i = 0; i < commands.Length; i++)
            {
                if (commands[i] == '[')
                    stack.Push(i);
                if (commands[i] == ']')
                {
                    loops.Add(stack.Peek(), i);
                    reversedLoops.Add(i, stack.Pop());
                }
            }
        }
    }
}