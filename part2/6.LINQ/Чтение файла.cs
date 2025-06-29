using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        public static IDictionary<int, SlideRecord> ParseSlideRecords(
            IEnumerable<string> lines) =>
          lines?.Skip(1)
                .Select(line => line.Split(';'))
                .Select(TryParseSlide)
                .Where(slide => slide != null)
                .ToDictionary(slide => slide.SlideId);

        private static SlideRecord TryParseSlide(string[] parts)
        {
            if ((parts.Length != 3) ||
            (!int.TryParse(parts[0], out int slideId)) ||
            (!Enum.TryParse(parts[1], true, out SlideType type))) return null;

            return new SlideRecord(slideId, type, parts[2]);
        }

        public static IEnumerable<VisitRecord> ParseVisitRecords(IEnumerable<string> lines,
            IDictionary<int, SlideRecord> slides) => lines?.Skip(1)
                                          .Select(line => ParseVisit(line, slides));

        private static VisitRecord ParseVisit(string line, IDictionary<int, SlideRecord> slides)
        {
            try
            {
                var parts = line.Split(';');
                if (parts.Length != 4) throw new FormatException();

                var userId = int.Parse(parts[0]);
                var slideId = int.Parse(parts[1]);
                var dateTime = DateTime.ParseExact(
                    $"{parts[2]} {parts[3]}",
                    "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture
                );
                var slydeType = slides[slideId].SlideType;

                return new VisitRecord(userId, slideId, dateTime, slydeType);
            }
            catch (Exception e)
            {
                throw new FormatException($"Wrong line [{line}]", e);
            }
        }
    }
}