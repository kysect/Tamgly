﻿using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class WorkItem : IEquatable<WorkItem>
{
    public Guid Id { get; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public WorkItemState State { get; set; }
    public DateTime CreationTime { get; }
    public virtual ICollection<WorkItemTrackInterval> Intervals { get; }
    public TimeSpan? Estimate { get; private set; }

    public static WorkItem Create(string title, string? description = null)
    {
        return new WorkItem(Guid.NewGuid(), title, description, WorkItemState.Open, DateTime.Now, new List<WorkItemTrackInterval>(), estimate: null);
    }

    public WorkItem(Guid id, string title, string? description, WorkItemState state, DateTime creationTime, ICollection<WorkItemTrackInterval> intervals, TimeSpan? estimate)
    {
        Id = id;
        Title = title;
        Description = description;
        State = state;
        CreationTime = creationTime;
        Intervals = intervals;
        Estimate = estimate;
    }

    public void UpdateInfo(string title, string? description = null)
    {
        Title = title;
        Description = description;
    }

    public void SetCompleted()
    {
        if (State != WorkItemState.Open)
            throw new TamglyException($"Work item already closed. Id: {Id}");

        State = WorkItemState.Closed;
    }

    public void AddInterval(WorkItemTrackInterval interval)
    {
        ArgumentNullException.ThrowIfNull(interval);

        Intervals.Add(interval);
    }

    public TimeSpan? GetIntervalSum()
    {
        IEnumerable<TimeSpan> timeSpans = Intervals
            .Select(i => i.GetDuration())
            .Where(d => d is not null)
            .Select(d => d.Value);

        TimeSpan? result = null;
        foreach (TimeSpan intervalPeriod in timeSpans)
        {
            if (result is null)
                result = intervalPeriod;
            else
                result = result.Value.Add(intervalPeriod);
        }

        return result;
    }

    public double? TryGetEstimateMatchPercent()
    {
        TimeSpan? intervalSum = GetIntervalSum();
        if (Estimate is null || intervalSum is null)
            return null;

        double minValue = Math.Min(Estimate.Value.TotalMinutes, intervalSum.Value.TotalMinutes);
        double maxValue = Math.Max(Estimate.Value.TotalMinutes, intervalSum.Value.TotalMinutes);
        return minValue / maxValue;
    }

    public bool Equals(WorkItem? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        
        if (ReferenceEquals(this, other))
            return true;
        
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is WorkItem item)
            return Equals(item);
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}