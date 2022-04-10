﻿using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public readonly struct TimeInterval : ITimeInterval
{
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public TimeInterval(DateOnly start, DateOnly end)
    {
        if (start > end)
            throw new TamglyException($"Interval end before start. Start: {start}, end: {end}");

        Start = start;
        End = end;
    }

    public bool Contains(DateOnly date)
    {
        return Start <= date && date <= End;
    }
}