using Kysect.Tamgly.ConsoleClient;
using Kysect.Tamgly.Core;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .WriteTo.File("log.log")
    .CreateLogger();

var proofOfConcept = new ProofOfConcept();
proofOfConcept.Execute();

void Samples()
{
    var workItemManager = new WorkItemManager();
    var workItemBuilder = new WorkItemBuilder("Support projects");
    WorkItem workItem = workItemBuilder.Build();
    workItemManager.AddWorkItem(workItem);

    var project = Project.Create("Tamgly");
    workItemManager.AddProject(project);
    workItemManager.ChangeProject(workItem, project);
    Console.WriteLine($"Project WI count: {project.GetAllWorkItemWithRepetitive().Count}");

    var backlogManager = new BacklogManager(workItemManager);
    DateOnly workItemDeadline = DateOnly.FromDateTime(DateTime.Today).AddDays(10);

    workItem = workItem with { Deadline = new WorkItemDeadline(new TamglyDay(workItemDeadline)) };
    workItemManager.UpdateWorkItem(workItem);

    DailyWorkItemBacklog workItemBacklog = backlogManager.GetDailyBacklog(workItemDeadline);
    Console.WriteLine($"Daily backlog WI count: {workItemBacklog.CurrentDay.Items.Count}");

    workItem = workItem with { Deadline = new WorkItemDeadline(new TamglyWeek(workItemDeadline)) };
    workItemManager.UpdateWorkItem(workItem);
    WeeklyWorkItemBacklog weeklyBacklog = backlogManager.GetWeeklyBacklog(workItemDeadline);
    Console.WriteLine($"Weekly backlog WI count: {weeklyBacklog.CurrentWeek.Items.Count}");

    workItem = workItem with { Deadline = new WorkItemDeadline(new TamglyMonth(workItemDeadline)) };
    workItemManager.UpdateWorkItem(workItem);
    MonthlyWorkItemBacklog monthlyBacklog = backlogManager.GetMonthlyBacklog(workItemDeadline);
    Console.WriteLine($"Monthly backlog WI count: {monthlyBacklog.CurrentMonth.Items.Count}");

    Console.Read();
}