using System;
using Avalonia.Input;
using Digger.Architecture;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

            if (Game.Map[x + command.DeltaX, y + command.DeltaY] is Digger.Sack)
                (command.DeltaX, command.DeltaY) = (0, 0);
            return command;
        }


        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack;
        }
    }

    class Sack : ICreature
    {
        public int FallCount = 0;
        public CreatureCommand Act(int x, int y)
        {
            if (CanFall(x, y)) return Fall();
            if (CanCrash(x, y)) return Crash();

            FallCount = 0;
            return new CreatureCommand();
        }

        public static CreatureCommand Crash()
        {
            return new CreatureCommand() { TransformTo = new Gold() };
        }

        public bool CanFall(int x, int y)
        {
            if (Game.MapHeight <= y + 1)
                return false;
            var cellBelow = Game.Map[x, y + 1];
            return (cellBelow is null) || ((FallCount > 0) && (cellBelow is Player));
        }

        public bool CanCrash(int x, int y)
        {
            return FallCount > 1;
        }

        public CreatureCommand Fall()
        {
            FallCount++;
            return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
        }


        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
                Game.Scores += 10;
            return conflictedObject is Player;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }
}