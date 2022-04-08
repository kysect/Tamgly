using System;
using Kysect.Tamgly.Core.Aggregates;
using Kysect.Tamgly.Core.Entities.Backlogs;
using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;
using NUnit.Framework;
using static Kysect.Tamgly.Core.ValueObjects.WorkItemDeadline;

namespace Kysect.Tamgly.Tests;

public class WorkItemBacklogTests
{
    private readonly BacklogManager _backlogManager;
    private readonly DateOnly _workItemDeadline;

    public WorkItemBacklogTests()
    {
        var workItemManager = new WorkItemManager();
        _backlogManager = new BacklogManager(workItemManager);
        _workItemDeadline = DateOnly.FromDateTime(new DateTime(2022, 04, 08));

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses")
                .SetDeadline(Create(WorkItemDeadlineType.Day, _workItemDeadline))
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 09")
                .SetDeadline(Create(WorkItemDeadlineType.Day, _workItemDeadline))
                .SetEstimates(TimeSpan.FromHours(3))
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Tamgly")
                .SetDeadline(Create(WorkItemDeadlineType.Week, _workItemDeadline))
                .SetEstimates(TimeSpan.FromHours(8))
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 10")
                .SetDeadline(Create(WorkItemDeadlineType.Week, _workItemDeadline))
                .SetEstimates(TimeSpan.FromHours(12))
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Lab 4/5")
                .SetDeadline(Create(WorkItemDeadlineType.Month, _workItemDeadline))
                .SetEstimates(TimeSpan.FromHours(25))
                .Build());
    }

    [Test]
    public void Test1()
    {
        DailyWorkItemBacklog workItemBacklog = _backlogManager.GetDailyBacklog(_workItemDeadline);
        WeeklyWorkItemBacklog weeklyBacklog = _backlogManager.GetWeeklyBacklog(_workItemDeadline);
        MonthlyWorkItemBacklog monthlyBacklog = _backlogManager.GetMonthlyBacklog(_workItemDeadline);

        Assert.AreEqual(TimeSpan.FromHours(3), workItemBacklog.TotalEstimate);
        Assert.AreEqual(TimeSpan.FromHours(20), workItemBacklog.TotalEstimateForWeek);
        Assert.AreEqual(TimeSpan.FromHours(20), weeklyBacklog.TotalEstimate);
        Assert.AreEqual(TimeSpan.FromHours(25), workItemBacklog.TotalEstimateForMonth);
        Assert.AreEqual(TimeSpan.FromHours(25), monthlyBacklog.TotalEstimateForMonth);
    }
}