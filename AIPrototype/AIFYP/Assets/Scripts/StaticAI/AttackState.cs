using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackState : GoapAction
{
    GoapAgent agentRef;
    public GameObject bullet;
    Vector3 dir;
    bool shotFired = false;
    public float fireRate = 1f;
    private float lastShotTime = -10f;

    private void Awake()
    {
        preconditions.Add("playerVisible", true);
        preconditions.Add("inAttackRange", true);
        effects.Add("KillPlayer", true );
        cost = 0f;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return Time.time >= lastShotTime + fireRate;
    }

    public override bool Perform(GoapAgent agent)
    {
        lastShotTime = Time.time;
        agentRef = agent;
        shotFired = false;
        agent.agent.ResetPath();

        dir = (agent.transforms.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        Debug.DrawLine(transform.position, agent.transforms.transform.position);
        Instantiate(bullet, transform.position + dir, Quaternion.LookRotation(dir));
        shotFired = true;

        return true;
    }

    public override bool IsDone()
    {
        return shotFired;
    }
}
