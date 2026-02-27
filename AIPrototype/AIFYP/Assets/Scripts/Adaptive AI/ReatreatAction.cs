using UnityEngine;
using UnityEngine.AI;

public class RetreatAction : GoapAction
{
    private Vector3 target;
    private bool reached;
    private CoverFinder coverFinder;

    public float attackRange = 10f;

    void Awake()
    {
        preconditions.Add("lowHealth", true);
        effects.Add("isSafe", true);
    }

    public override bool Perform(GoapAgent agent)
    {
        if (coverFinder == null)
            coverFinder = agent.GetComponent<CoverFinder>();

        // pick a target cover if none exists
        if (target == Vector3.zero)
        {
            target = coverFinder.FindCoverPoint(agent.transform.position, agent.Player.position);
            reached = false;
        }

        agent.agent.SetDestination(target);

        // attack while retreating
        float distToPlayer = Vector3.Distance(agent.transform.position, agent.player.position);
        if (distToPlayer <= attackRange)
        {
            // implement your shooting logic here
            // e.g., agent.ShootAt(agent.player.position);
        }

        // check if reached cover
        if (!agent.agent.pathPending && agent.agent.remainingDistance < 0.5f)
        {
            reached = true;
        }

        return true;
    }

    public override bool IsDone()
    {
        if (reached)
        {
            var health = GetComponent<Health>();
            if (health.CurrentHealth >= health.MaxHealth * 0.6f) // healed enough
            {
                target = Vector3.zero;
                reached = false;
                return true;
            }
        }

        return false;
    }
}