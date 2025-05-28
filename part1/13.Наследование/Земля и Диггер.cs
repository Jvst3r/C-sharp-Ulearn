using System;
using Avalonia.Input;
using Digger.Architecture;

namespace Digger
{
    //Напишите здесь классы Player, Terrain и другие.
    class Terrain : ICreature
    {
        public string GetImageFileName() //вроде сделано
        {
            return "Terrain.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }
    }

    class Player : ICreature
    {
        public string GetImageFileName()
        {
            return "Digger.png";
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand();
            switch (Game.KeyPressed)
            {
                case Key.Up:
                    if (y - 1 >= 0)
                        command.DeltaY = -1;
                    break;
                case Key.Down:
                    if (y + 1 < Game.MapHeight)
                        command.DeltaY = 1;
                    break;
                case Key.Left:
                    if (0 <= x - 1)
                        command.DeltaX = -1;
                    break;
                case Key.Right:
                    if (x + 1 < Game.MapWidth)
                        command.DeltaX = 1;
                    break;
                default:
                    break;
            }

            return command;
        }


        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }
    }
}