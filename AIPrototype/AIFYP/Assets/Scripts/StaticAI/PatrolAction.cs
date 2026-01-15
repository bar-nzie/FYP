using UnityEngine;
using UnityEngine.AI;

public class PatrolAction : GoapAction
{
    Vector3 target;

    private void Awake()
    {
        preconditions.Add("playerVisible", false);
        effects.Add("isAtPatrolPoint", true);
        cost = 2f;
    }

    public override bool Perform(GoapAgent agent)
    {
        if (target == Vector3.zero)
        {
            Vector3 random = Random.insideUnitSphere * 10f;
            random += agent.transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(random, out hit, 10f, NavMesh.AllAreas);
            target = hit.position;

            agent.agent.SetDestination(target);
        }
        return true;
    }

    public override bool IsDone()
    {
        return Vector3.Distance(transform.position, target) < 1f;
    }
}
