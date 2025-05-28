namespace Mazes;

public static class EmptyMazeTask
{
    // перемещение по горизонтали (координата Х)
    public static void DoHorizontalMove(Robot robot, int width, Direction direction)
    {
        for (int i = 0; i < width - 3; i++)
            robot.MoveTo(direction);
    }

    // перемещение по вертикали (координата У)
    public static void DoVerticalMove(Robot robot, int height, Direction direction)
    {
        for (int i = 0; i < height - 3; i++)
            robot.MoveTo(direction);
    }

    public static void MoveOut(Robot robot, int width, int height)
    {
        DoHorizontalMove(robot, width, Direction.Right);
        DoVerticalMove(robot, height, Direction.Down);
    }
}