using UnityEngine;
using UnityEngine.AI;

public class AdaptiveChaseAction : GoapAction
{
    private bool reached;
    private Vector3 target;

    public float sneakSpeed = 3f;
    public float chaseSpeed = 7f;
    public float flankDistance = 2f; // distance to flank the player when sneaking

    void Awake()
    {
        preconditions.Add("playerVisible", true); // only chase if the AI can see the player
        effects.Add("inAttackRange", true);       // goal is to reach attack range
    }

    public override bool Perform(GoapAgent agent)
    {
        if (agent.player == null) return false;

        // Determine if the player can see the AI
        bool playerAwareOfAI = agent.world.Get("playerVisible"); // you can also add a separate "playerAwareOfAI" if needed

        if (!playerAwareOfAI)
        {
            // Sneak approach
            agent.agent.speed = sneakSpeed;

            // pick a point slightly behind/flanking the player
            Vector3 directionToPlayer = (agent.player.position - agent.transform.position).normalized;
            Vector3 flankOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            target = agent.player.position - directionToPlayer * flankDistance + flankOffset;
        }
        else
        {
            // Aggressive chase
            agent.agent.speed = chaseSpeed;
            target = agent.player.position;
        }

        agent.agent.SetDestination(target);

        // Check if AI is close enough to attack
        if (!agent.agent.pathPending && agent.agent.remainingDistance < agent.attackRange)
        {
            reached = true;
        }

        return true;
    }

    public override bool IsDone()
    {
        return reached;
    }
}