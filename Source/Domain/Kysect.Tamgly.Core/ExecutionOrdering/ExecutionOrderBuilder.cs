﻿using Serilog;

namespace Kysect.Tamgly.Core;

public class ExecutionOrderBuilder
{
    private DateOnly _currentDay;

    private readonly SelectedDayOfWeek _selectedDayOfWeek;
    private readonly TimeSpan _workingHoursPerDay;
    private readonly List<DailyAssignments> _assignedWorkItems;

    public ExecutionOrderBuilder(DateOnly currentDay, SelectedDayOfWeek selectedDayOfWeek, TimeSpan workingHoursPerDay)
    {
        if (selectedDayOfWeek == SelectedDayOfWeek.None)
            throw new TamglyException($"Cannot create Daily assignments because no work day selected");

        _currentDay = currentDay;
        _selectedDayOfWeek = selectedDayOfWeek;
        _workingHoursPerDay = workingHoursPerDay;
        _assignedWorkItems = new List<DailyAssignments>();
    }

    public void Add(WorkItem workItem)
    {
        if (workItem.Estimate is null)
        {
            Log.Information($"Skip WI {workItem.ToShortString()} in execution order building. WI do not have estimates");
            return;
        }

        DailyAssignments dailyAssignmentsWithFreeTime = GetDailyAssignmentsWithFreeTime(workItem.Estimate.Value);
        dailyAssignmentsWithFreeTime.WorkItems.Add(workItem);
    }

    public DailyAssignments GetDailyAssignmentsWithFreeTime(TimeSpan estimates)
    {
        if (estimates > _workingHoursPerDay)
            throw new TamglyException($"Cannot find time for Work item. Estimates is bigger that time for a one day.");

        if (!_assignedWorkItems.Any())
            return CreateNewDailyAssignment(_currentDay);

        DailyAssignments lastAssignment = _assignedWorkItems.Last();
        if (lastAssignment.TotalEstimates() + estimates > _workingHoursPerDay)
            return CreateNewDailyAssignment(_currentDay.AddDays(1));

        return lastAssignment;
    }

    private DailyAssignments CreateNewDailyAssignment(DateOnly searchFrom)
    {
        _currentDay = GetNextDayFromSelectedDayOfWeek(searchFrom);
        var dailyAssignments = new DailyAssignments(_currentDay, new List<WorkItem>());
        _assignedWorkItems.Add(dailyAssignments);
        return dailyAssignments;
    }

    private DateOnly GetNextDayFromSelectedDayOfWeek(DateOnly searchFrom)
    {
        while (!_selectedDayOfWeek.Contains(searchFrom))
        {
            searchFrom = searchFrom.AddDays(1);
        }

        return searchFrom;
    }
}