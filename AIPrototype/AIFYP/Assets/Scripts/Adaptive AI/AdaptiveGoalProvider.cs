using System.Collections.Generic;
using UnityEngine;

public class AdaptiveGoalProvider : MonoBehaviour, IGoalProvider
{
    private ExplorationMemory memory;
    private GoapAgent agent;
    private Health health;

    private void Awake()
    {
        agent = GetComponent<GoapAgent>();
        memory = GetComponent<ExplorationMemory>();
        health = GetComponent<Health>();
    }

    public Dictionary<string, bool> GetGoal(WorldState world)
    {
        var goal = new Dictionary<string, bool>();

        bool playerVisible = world.Get("playerVisible");
        bool inAttackRange = world.Get("inAttackRange");
        bool lowHealth = world.Get("lowHealth");


        if (lowHealth)
        {
            goal["isSafe"] = true;          // triggers RetreatAction
        }

        else if (playerVisible && inAttackRange)
        {
            goal["inAttackRange"] = true;   // triggers AttackAction
        }

        else if (playerVisible)
        {
            goal["playerVisible"] = true;   // triggers MoveToAction / ChaseAction
        }

        else
        {
            goal["isSearching"] = true;     // triggers SearchAction
        }
        if (world.Get("lowHealth") && world.Get("playerVisible"))
        {
            goal["isSafe"] = true; // retreat goal
            goal["playerVisible"] = true; // attack while retreating
        }

        return goal;
    }
}