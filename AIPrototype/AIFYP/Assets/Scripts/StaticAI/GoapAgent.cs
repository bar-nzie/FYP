using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class GoapAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject transforms;
    public Transform player;
    public Transform Player => player;
    public float attackRange = 2f;
    private Health health;

    public WorldState world = new();
    GoapPlanner planner = new();
    Queue<GoapAction> currentPlan;

    List<GoapAction> actions;
    private IGoalProvider goalProvider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actions = new List<GoapAction>(GetComponents<GoapAction>());
        transforms = GameObject.Find("Player");
        player = transforms.transform;
        goalProvider = GetComponent<IGoalProvider>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWorldState();

        if(currentPlan == null || currentPlan.Count == 0) MakePlan();
        if (currentPlan == null || currentPlan.Count == 0) return;

        var action = currentPlan.Peek();

        if(action.Perform(this))
        {
            if (action.IsDone()) currentPlan.Dequeue();
        }
    }

    void MakePlan()
    {
        if (goalProvider == null) return;

        var goal = goalProvider.GetGoal(world);
        currentPlan = planner.Plan(actions, world.GetStates(), goal);
    }

    void UpdateWorldState()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        world.Set("playerVisible", dist < 10f);
        world.Set("inAttackRange", dist < attackRange);

        if (health != null)
        {
            world.Set("lowHealth", health.CurrentHealth < health.MaxHealth * 0.41f);
            world.Set("isDead", health.IsDead);
        }

        world.Set("lastKnownPlayerPosition", true);
        world.Set("safePositionReached", false);
        world.Set("isSafe", false);
    }
}
