using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        if (NullEmptyCheck(visits))
            return 0.0;

        var timeDiff = CalculateDifferencesInTimePerSlide(visits, slideType);

        if (timeDiff.Count == 0)
            return 0.0;

        return CalculateMedian(timeDiff);
    }

    private static double CalculateMedian(List<double> values)
    {
        if (values == null || values.Count == 0) return 0.0;

        var sorted = values.OrderBy(value => value).ToArray();
        var count = sorted.Length;
        int center = count / 2;
        double median;
        median = (count % 2 == 0)
           ? (sorted[center - 1] + sorted[center]) / 2.0
           : sorted[center];

        return median;
    }

    private static bool NullEmptyCheck(List<VisitRecord> visits) => visits.Count == 0 || visits == null;

    private static List<double> CalculateDifferencesInTimePerSlide(List<VisitRecord> visits, SlideType slideType) =>
        visits?
        .GroupBy(v => v.UserId)
        .SelectMany(userVisits => ProcessUserVisits(userVisits.OrderBy(v => v.DateTime).ToList(), slideType))
        .ToList() ?? new List<double>();

    private static List<double> ProcessUserVisits(List<VisitRecord> orderedVisits, SlideType targetSlideType) =>
            orderedVisits
            .Zip(orderedVisits.Skip(1), (current, next) => (current, next))
            .Where(pair => IsValidTransition(pair.current, pair.next, targetSlideType))
            .Select(pair => (pair.next.DateTime - pair.current.DateTime).TotalMinutes)
            .Where(minutes => minutes is >= 1 and <= 120)
            .ToList();

    private static bool IsValidTransition(VisitRecord current, VisitRecord next, SlideType targetSlideType) =>
               current.SlideType == targetSlideType &&
                current.SlideId != next.SlideId;
}