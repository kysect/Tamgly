using Kysect.Tamgly.Common;
using Serilog;

namespace Kysect.Tamgly.Core;

public class WorkItemManager : IWorkItemManager
{
    private readonly Project _defaultProject;
    private readonly WorkItemManagerConfig _config;
    private readonly ICollection<Project> _projects;

    public WorkItemManager()
        : this(new WorkItemManagerConfig(), new List<Project>())
    {
    }

    public WorkItemManager(WorkItemManagerConfig config, ICollection<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(projects);

        _config = config;
        _projects = projects;
        _defaultProject = new Project(Guid.Empty, "Default project", new List<WorkItem>(), new List<RepetitiveParentWorkItem>(), WorkingHours.Empty);
        _projects.Add(_defaultProject);
    }

    public void AddWorkItem(WorkItem item, Project? project = null)
    {
        ArgumentNullException.ThrowIfNull(item);

        Log.Debug($"Add WI {item.Id.ToShortString()} to WIManager");

        _defaultProject.AddItem(item);
        
        if (project is not null)
            ChangeProject(item, project);
    }

    public void AddWorkItem(RepetitiveParentWorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Log.Debug($"Add repetitive WI {item.Id.ToShortString()} to WIManager");

        _defaultProject.AddItem(item);
    }

    public void RemoveWorkItem(WorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Project project = GetProject(item);
        project.RemoveItem(item);
    }

    public void UpdateWorkItem(WorkItem item)
    {
        Log.Debug($"Update WI in WIManager: {item.ToShortString()}");

        Project? project = FindProject(item);
        if (project is null)
            return;

        project.RemoveItem(item);
        project.AddItem(item);
    }

    public void AddProject(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);

        Log.Debug($"Add new project {project.ToShortString()} to WIManager");

        _projects.Add(project);
    }

    public void RemoveProject(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);

        if (project.Id == _defaultProject.Id)
            throw new TamglyException("Cannot remove default project.");

        if (!_projects.Remove(project))
            throw new TamglyException($"Project was not found. Id: {project.Id}");

        foreach (WorkItem projectItem in project.Items)
            _defaultProject.AddItem(projectItem);

        foreach (RepetitiveParentWorkItem projectItem in project.RepetitiveItems)
            _defaultProject.AddItem(projectItem);
    }

    public void ChangeProject(WorkItem item, Project project)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(project);

        Log.Debug($"Change project for {item.ToShortString()} to {project.ToShortString()}");

        Project oldProject = GetProject(item);
        oldProject.RemoveItem(item);
        project.AddItem(item);
    }

    public ICollection<Project> GetAllProjects()
    {
        return _projects;
    }

    public IReadOnlyCollection<WorkItem> GetSelfWorkItems()
    {
        return _projects
            .SelectMany(p => p.GetAllWorkItemWithRepetitive())
            .Where(w => w.AssignedTo.IsMe())
            .ToList();
    }

    public IReadOnlyCollection<WorkItem> GetAllWorkItems()
    {
        return _projects
            .SelectMany(p => p.GetAllWorkItemWithRepetitive())
            .ToList();
    }

    public IReadOnlyCollection<WorkItem> GetWorkItemsWithWrongEstimates()
    {
        return GetSelfWorkItems()
            .Where(HasWrongEstimate)
            .ToList();

        bool HasWrongEstimate(WorkItem workItem)
        {
            double? matchPercent = workItem.TryGetEstimateMatchPercent();
            return matchPercent is not null
                   && matchPercent < _config.AcceptableEstimateDiff;
        }
    }

    private Project GetProject(WorkItem workItem)
    {
        return FindProject(workItem) ?? throw new TamglyException($"Work item was not matched with any project. Id: {workItem.Id}");
    }

    // TODO: WI32 for future optimizations
    private Project? FindProject(WorkItem workItem)
    {
        return _projects.SingleOrDefault(p => p.Items.Contains(workItem));
    }
}