using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
using System.Linq;

public class GoapPlanner
{
    class Node
    {
        public Node parent;
        public float cost;
        public Dictionary<string, bool> state;
        public GoapAction action;
    }

    public Queue<GoapAction> Plan(List<GoapAction> actions, Dictionary<string, bool> worldState, Dictionary<string, bool> goal)
    {
        List<Node> leaves = new();
        Node start = new() { state = worldState, cost = 0 };

        bool success = BuildGraph(start, leaves, actions, goal);
        if (!success) return null;

        Node cheapest = leaves.OrderBy(n => n.cost).First();
        var result = new List<GoapAction>();

        while (cheapest.action != null)
        {
            result.Insert(0, cheapest.action);
            cheapest = cheapest.parent;
        }

        return new Queue<GoapAction>(result);
    }

    bool BuildGraph(Node parent, List<Node> leaves, List<GoapAction> actions, Dictionary<string, bool> goal)
    {
        bool foundPath = false;

        foreach (var action in actions)
        {
            if (!InState(action.preconditions, parent.state)) continue;

            var newState = new Dictionary<string, bool>(parent.state);
            foreach (var effect in action.effects) newState[effect.Key] = effect.Value;

            Node node = new()
            {
                parent = parent,
                cost = parent.cost + action.cost,
                state = newState,
                action = action
            };

            if(InState(goal, newState))
            {
                leaves.Add(node);
                foundPath = true;
            }
            else
            {
                var subset = actions.Where(a => a != action).ToList();
                if(BuildGraph(node, leaves, subset, goal)) foundPath = true;
            }
        }
        return foundPath;
    }

    bool InState(Dictionary<string, bool> test, Dictionary<string, bool> state)
    {
        foreach(var t in test)
        {
            if (!state.ContainsKey(t.Key) || state[t.Key] != t.Value) return false;
        }
        return true;
    }
}
