using UnityEngine;

public class ChaseAction : GoapAction
{
    GoapAgent agentRef;

    private void Awake()
    {
        preconditions.Add("playerVisible", true);
        preconditions.Add("inAttackRange", false);
        effects.Add("inAttackRange", true);
        cost = 1f;
    }

    public override bool Perform(GoapAgent agent)
    {
        agentRef = agent;
        agent.agent.SetDestination(agent.transforms.transform.position);
        return true;
    }

    public override bool IsDone()
    {
        return Vector3.Distance(transform.position, agentRef.transform.position) < agentRef.attackRange;
    }
}
