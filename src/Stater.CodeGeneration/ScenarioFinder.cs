using Stater.Domain.Models;

namespace Stater.CodeGeneration;

internal record MultipleGuid(
    Guid Guid1,
    Guid Guid2
);

public static class ScenarioFinder
{
    public static List<List<Transition>> FindScenarios(StateMachine stateMachine)
    {
        if (stateMachine.StartState == null) return new List<List<Transition>>();
        var result = FindAllPaths(stateMachine.Transitions, stateMachine.StartState.Guid);
        return result
            .Where(el => el.Count > 0)
            .DistinctBy(el => el
                .Select(t => t.Guid.GetHashCode())
                .Aggregate(0, HashCode.Combine)
            )
            .OrderByDescending(el => el.Count)
            .Take(3)
            .ToList();
    }

    private static IEnumerable<List<Transition>> FindAllPaths(List<Transition> transitions, Guid start)
    {
        var graph = BuildGraph(transitions);
        var paths = new List<List<Transition>>();
        var visited = new HashSet<MultipleGuid>();
        Dfs(graph, transitions, null, start, new List<Transition>(), visited, paths);
        return paths;
    }

    private static Dictionary<Guid, List<Guid>> BuildGraph(List<Transition> transitions)
    {
        var graph = new Dictionary<Guid, List<Guid>>();

        foreach (var transition in
                 transitions.Where(transition => transition is { Start: not null, End: not null }))
        {
            if (!graph.ContainsKey(transition.Start!.Value))
                graph[transition.Start.Value] = new List<Guid>();

            graph[transition.Start.Value].Add(transition.End!.Value);
        }

        return graph;
    }

    private static void Dfs(
        IReadOnlyDictionary<Guid, List<Guid>> graph,
        IReadOnlyCollection<Transition> transitions,
        Guid? prev,
        Guid current,
        IList<Transition> path,
        ISet<MultipleGuid> visited,
        ICollection<List<Transition>> paths)
    {
        if (prev != null)
            visited.Add(new MultipleGuid((Guid)prev, current));

        var isEnd = true;
        if (graph.TryGetValue(current, out var value))
        {
            foreach (var neighbor in value)
            {
                if (visited.Contains(new MultipleGuid(current, neighbor))) continue;
                var transition = transitions.First(t => t.Start == current && t.End == neighbor);
                path.Add(transition);
                Dfs(graph, transitions, current, neighbor, path, visited, paths);
                path.RemoveAt(path.Count - 1);
                isEnd = false;
            }
        }

        if (isEnd)
            paths.Add(new List<Transition>(path));

        if (prev != null)
            visited.Remove(new MultipleGuid((Guid)prev, current));
    }
}