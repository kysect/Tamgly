using Serilog;

namespace Kysect.Tamgly.Core;

public class BacklogManager
{
    private readonly IWorkItemManager _itemManager;

    public BacklogManager(IWorkItemManager itemManager)
    {
        ArgumentNullException.ThrowIfNull(itemManager);

        _itemManager = itemManager;
    }

    public DailyWorkItemBacklog GetDailyBacklog(DateOnly time)
    {
        IReadOnlyCollection<WorkItem> workItems = _itemManager.GetSelfWorkItems();

        Log.Debug($"Create daily backlog for {time}. Work items count: {workItems.Count}");

        return DailyWorkItemBacklog.Create(workItems, time);
    }

    public WeeklyWorkItemBacklog GetWeeklyBacklog(DateOnly time)
    {
        IReadOnlyCollection<WorkItem> workItems = _itemManager.GetSelfWorkItems();

        Log.Debug($"Create daily backlog for {time}. Work items count: {workItems.Count}");

        return WeeklyWorkItemBacklog.Create(workItems, time);
    }

    public MonthlyWorkItemBacklog GetMonthlyBacklog(DateOnly time)
    {
        IReadOnlyCollection<WorkItem> workItems = _itemManager.GetSelfWorkItems();

        Log.Debug($"Create daily backlog for {time}. Work items count: {workItems.Count}");

        return MonthlyWorkItemBacklog.Create(workItems, time);
    }
}