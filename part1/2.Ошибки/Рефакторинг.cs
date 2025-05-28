using Avalonia.Media;
using RefactorMe.Common;
using System;
using System.Security.Cryptography;
// изменил названия методов по правилам языка (начинаются с прописной буквы),
// названия переменных с русского транслита на англиские словалатиницей с маленькой буквы
//Общие правила разделения слов в названии одного метода или переменной - прописная буква (diagonalLen переменная
// или MakeIt метод)
//каждую из сторон теперь рисует отдельный метод

namespace RefactorMe
{
    class Drawer
    {
        static float x, y;
        static IGraphics graphics;

        public static void DrawingSide1(int width, int height)
        {
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f, 0);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.04f * Math.Sqrt(2), Math.PI / 4);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f, Math.PI);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f - (Math.Min(width, height)) * 0.04f, Math.PI / 2);

            Drawer.Change((Math.Min(width, height)) * 0.04f, -Math.PI);
            Drawer.Change((Math.Min(width, height)) * 0.04f * Math.Sqrt(2), 3 * Math.PI / 4);
        }

        public static void DrawingSide2(int width, int height)
        {
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f, -Math.PI / 2);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.04f * Math.Sqrt(2), -Math.PI / 2 + Math.PI / 4);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f, -Math.PI / 2 + Math.PI);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f - (Math.Min(width, height)) * 0.04f, -Math.PI / 2 + Math.PI / 2);

            Drawer.Change((Math.Min(width, height)) * 0.04f, -Math.PI / 2 - Math.PI);
            Drawer.Change((Math.Min(width, height)) * 0.04f * Math.Sqrt(2), -Math.PI / 2 + 3 * Math.PI / 4);
        }


        public static void DrawingSide3(int width, int height)
        {
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f, Math.PI);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.04f * Math.Sqrt(2), Math.PI + Math.PI / 4);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f, Math.PI + Math.PI);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f - (Math.Min(width, height)) * 0.04f, Math.PI + Math.PI / 2);

            Drawer.Change((Math.Min(width, height)) * 0.04f, Math.PI - Math.PI);
            Drawer.Change((Math.Min(width, height)) * 0.04f * Math.Sqrt(2), Math.PI + 3 * Math.PI / 4);
        }

        public static void DrawingSide4(int width, int height)
        {
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f, Math.PI / 2);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.04f * Math.Sqrt(2), Math.PI / 2 + Math.PI / 4);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f, Math.PI / 2 + Math.PI);
            Drawer.MakeIt(new Pen(Brushes.Yellow), (Math.Min(width, height)) * 0.375f - (Math.Min(width, height)) * 0.04f, Math.PI / 2 + Math.PI / 2);

            Drawer.Change((Math.Min(width, height)) * 0.04f, Math.PI / 2 - Math.PI);
            Drawer.Change((Math.Min(width, height)) * 0.04f * Math.Sqrt(2), Math.PI / 2 + 3 * Math.PI / 4);
        }

        public static void Initialize(IGraphics newGrafics)
        {
            graphics = newGrafics;
            //graphics.SmoothingMode = SmoothingMode.None;
            graphics.Clear(Colors.Black);
        }

        public static void SetPosition(float x0, float y0)
        { x = x0; y = y0; }

        public static void MakeIt(Pen pen, double len, double angle)
        {
            //Делает шаг длиной len в направлении angle и рисует пройденную траекторию
            var x1 = (float)(x + len * Math.Cos(angle));
            var y1 = (float)(y + len * Math.Sin(angle));
            graphics.DrawLine(pen, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void Change(double len, double angle)
        {
            x = (float)(x + len * Math.Cos(angle));
            y = (float)(y + len * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        public static void Draw(int width, int height, double angleRotation, IGraphics graphics)
        {
            // angleRotation пока не используется, но будет использоваться в будущем
            Drawer.Initialize(graphics);

            var diagonalLen = Math.Sqrt(2) * ((Math.Min(width, height)) * 0.375f + (Math.Min(width, height)) * 0.04f) / 2;
            var x0 = (float)(diagonalLen * Math.Cos(Math.PI / 4 + Math.PI)) + width / 2f;
            var y0 = (float)(diagonalLen * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;

            Drawer.SetPosition(x0, y0);
            //Рисуем i-ую сторону
            Drawer.DrawingSide1(width, height);
            Drawer.DrawingSide2(width, height);
            Drawer.DrawingSide3(width, height);
            Drawer.DrawingSide4(width, height);
        }
    }
}

