using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SearchAction : GoapAction
{
    private bool reached;
    private Vector3 target;
    private ExplorationMemory memory;
    private MapSearchPoints mapPoints;

    void Awake()
    {
        preconditions.Add("playerVisible", false);
        effects.Add("isSearching", true);
    }

    public override bool Perform(GoapAgent agent)
    {
        if (memory == null)
            memory = agent.GetComponent<ExplorationMemory>();

        if (mapPoints == null)
            mapPoints = GameObject.FindObjectOfType<MapSearchPoints>();

        if (mapPoints == null || mapPoints.searchPoints.Count == 0)
            return false;

        // **Check if player is visible or in attack range**
        if (agent.world.Get("playerVisible") || agent.world.Get("inAttackRange"))
        {
            // abandon search immediately
            reached = true;
            target = Vector3.zero;
            return true;
        }

        // Pick a new nearby target if we don't have one
        if (target == Vector3.zero)
        {
            target = memory.GetNextSearchPoint(mapPoints.searchPoints, agent.transform.position);
            reached = false; // reset reached only when a new target is picked
        }

        // Move toward the target
        agent.agent.SetDestination(target);

        // Check if AI reached the target
        if (!agent.agent.pathPending && agent.agent.remainingDistance < 0.5f)
        {
            reached = true;
            target = Vector3.zero; // next time Perform is called, a new point will be picked
        }

        return true;
    }

    public override bool IsDone()
    {
        return reached;
    }
}