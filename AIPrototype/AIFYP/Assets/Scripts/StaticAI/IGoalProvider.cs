using System.Collections.Generic;

public interface IGoalProvider
{
    Dictionary<string, bool> GetGoal(WorldState world);
}