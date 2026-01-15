using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class GoapAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject transforms;
    private Transform player;
    public float attackRange = 2f;

    WorldState world = new();
    GoapPlanner planner = new();
    Queue<GoapAction> currentPlan;

    List<GoapAction> actions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actions = new List<GoapAction>(GetComponents<GoapAction>());
        transforms = GameObject.Find("Player");
        player = transforms.transform;
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
        Dictionary<string, bool> goal;

        if (world.Get("playerVisible"))
        {
            goal = new Dictionary<string, bool>()
            {
                { "playerVisible", true },
                { "inAttackRange", true }
            };
        }
        else
        {
            goal = new Dictionary<string, bool>()
            {
                { "isAtPatrolPoint", true }
            };
        }

        currentPlan = planner.Plan(actions, world.GetStates(), goal);
    }

    void UpdateWorldState()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        world.Set("playerVisible", dist < 10f);
        world.Set("inAttackRange", dist < attackRange);
    }
}
