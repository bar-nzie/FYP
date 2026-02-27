using UnityEngine;
using System.Collections.Generic;

public class StaticGoalProvider : MonoBehaviour, IGoalProvider
{
    public Dictionary<string, bool> GetGoal(WorldState world)
    {
        if (world.Get("playerVisible"))
        {
            return new Dictionary<string, bool>
            {
                { "playerVisible", true },
                { "inAttackRange", true }
            };
        }

        return new Dictionary<string, bool>
        {
            { "isAtPatrolPoint", true }
        };
    }
}