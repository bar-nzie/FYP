using UnityEngine;

public class AttackState : GoapAction
{
    GoapAgent agentRef;

    private void Awake()
    {
        preconditions.Add("playerVisible", true);
        preconditions.Add("inAttackRange", true);
        effects.Add("KillPlayer", true );
        cost = 0f;
    }

    public override bool Perform(GoapAgent agent)
    {
        agentRef = agent;
        agent.agent.ResetPath();

        Vector3 dir = (agent.transforms.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        Debug.DrawLine(transform.position, agent.transforms.transform.position);

        return true;
    }

    public override bool IsDone()
    {
        return Vector3.Distance(transform.position, agentRef.transforms.transform.position) > agentRef.attackRange;
    }
}
