﻿using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

public record DailyAssignments(DateOnly Date, List<WorkItem> WorkItems)
{
    public TimeSpan TotalEstimates()
    {
        return WorkItems.Sum(item => item.Estimate) ?? TimeSpan.Zero;
    }
}