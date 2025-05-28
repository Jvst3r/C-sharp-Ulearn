namespace Mazes;

public static class DiagonalMazeTask
{
    public static void DoHorizontalMove(Robot robot, int width, Direction direction)
    {
        for (int i = 0; i < width; i++)
            robot.MoveTo(direction);
    }

    // перемещение по вертикали (координата У)
    public static void DoVerticalMove(Robot robot, int height, Direction direction)
    {
        for (int i = 0; i < height; i++)
            robot.MoveTo(direction);
    }

    public static void GoRightAndDown(Robot robot, int width, int height) //ход вправо-вниз
    {
        for (int i = 0; i < height - 2; i++)
        {
            DoVerticalMove(robot, width / (height - 1), Direction.Right);
            if (i != (height - 3)) // чтоб сходить не в стену
                DoHorizontalMove(robot, 1, Direction.Down);
        }
    }

    public static void GoDownAndRight(Robot robot, int width, int height)
    {
        for (int i = 0; i < width - 2; i++)
        {
            DoVerticalMove(robot, (height - 3) / (width - 2), Direction.Down);
            if (i != (width - 3)) // чтоб сходить не в стену
                DoHorizontalMove(robot, 1, Direction.Right);
        }
    }

    public static void MoveOut(Robot robot, int width, int height)
    {
        if (width > height)
            GoRightAndDown(robot, width, height);
        else
            GoDownAndRight(robot, width, height);
    }
}