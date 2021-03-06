namespace Kysect.Tamgly.Graphs;

public class GraphValueResolver<T> : IGraphValueResolver<T>
{
    private readonly Dictionary<Guid, T> _map;

    public GraphValueResolver(IReadOnlyCollection<T> values, Func<T, Guid> selector)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(selector);

        _map = values.ToDictionary(selector, v => v);
    }

    public T Resolve(Guid id)
    {
        if (_map.TryGetValue(id, out T? result))
            return result;

        throw new ArgumentException($"Graph node with id {id} was not found");
    }
}