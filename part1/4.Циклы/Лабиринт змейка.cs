namespace Mazes;

public static class SnakeMazeTask
{
    // перемещение по горизонтали (координата Х)
    public static void DoHorizontalMove(Robot robot, int width, Direction direction)
    {
        for (int i = 0; i < width - 3; i++)
            robot.MoveTo(direction);
    }

    // этот метод отвечает за "ходьбу" справа налево (жесткий полицейский разворот)
    public static void DoTurn(Robot robot, int width, int height) //width - длина, height - высота
    {
        //DoHorizontalMove(robot, width, Direction.Right);
        robot.MoveTo(Direction.Down); //идем вниз для разворота
        robot.MoveTo(Direction.Down); //идем вниз для разворота
        DoHorizontalMove(robot, width, Direction.Left); //идем справа налево
    }

    public static void MoveOut(Robot robot, int width, int height)
    {
        int CountOfWays = (height - 2) / 2 + 1; //количество горизонтальных "дорог"
        for (int i = 0; i < CountOfWays; i++)
        {
            if (i % 2 == 0)
                DoHorizontalMove(robot, width, Direction.Right);
            else if (i % 2 == 1 && CountOfWays - i > 1) //если это не последняя "дорога" для 				//разворота
            {
                DoTurn(robot, width, height);
                robot.MoveTo(Direction.Down); //идем вниз для разворота
                robot.MoveTo(Direction.Down); //идем вниз для разворота
            }
            else
                DoTurn(robot, width, height);
        }
    }
}