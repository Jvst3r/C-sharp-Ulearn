using System;
using System.Collections.Generic;
using System.Drawing;
using Avalonia.Media;
using Geometry;
using GeometryTasks;
namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        private static Dictionary<Segment, Color> _dic = new Dictionary<Segment, Color>();

        public static void SetColor(this Segment s, Color color)
        {
            if (_dic.ContainsKey(s))
                _dic[s] = color;
            else
                _dic.Add(s, color);
        }

        public static Color GetColor(this Segment s)
        {
            if (_dic.ContainsKey(s))
                return _dic[s];
            else
                return Colors.Black;
        }
    }
}