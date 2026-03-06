using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class GoapAgent : MonoBehaviour
{
    [Header("Components")]
    public NavMeshAgent agent;
    public Transform player;
    public Health health;

    [Header("Settings")]
    public float attackRange = 10f;
    public float sightRadius = 30f;     // how far AI can detect the player
    public float peripheralAngle = 90f; // FOV half-angle for player awareness

    // World state
    public WorldState world = new();
    private GoapPlanner planner = new();
    private Queue<GoapAction> currentPlan;
    private List<GoapAction> actions;
    private IGoalProvider goalProvider;

    // Expose player for actions
    public Transform Player => player;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (health == null) health = GetComponent<Health>();

        actions = new List<GoapAction>(GetComponents<GoapAction>());
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        goalProvider = GetComponent<IGoalProvider>();
    }

    void Update()
    {
        UpdateWorldState();

        if (currentPlan == null || currentPlan.Count == 0) MakePlan();
        if (currentPlan == null || currentPlan.Count == 0) return;

        var action = currentPlan.Peek();

        if (action.Perform(this))
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
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // --- Player detection ---
        bool playerVisible = dist < sightRadius && IsPlayerVisible();
        world.Set("playerVisible", playerVisible);
        world.Set("inAttackRange", dist < attackRange);

        // --- Health ---
        if (health != null)
        {
            world.Set("lowHealth", health.CurrentHealth < health.MaxHealth * 0.41f);
            world.Set("isDead", health.IsDead);
        }

        // --- Misc world states ---
        world.Set("lastKnownPlayerPosition", playerVisible); // could be improved with actual last known pos
        world.Set("safePositionReached", false);
        world.Set("isSafe", false);
    }

    // Checks if the AI is within the player's viewport & not blocked by walls
    bool IsPlayerVisible()
    {
        Camera playerCam = Camera.main; // assumes main camera is player
        if (playerCam == null) return false;

        // Check viewport
        Vector3 viewportPoint = playerCam.WorldToViewportPoint(transform.position);
        bool inViewport = viewportPoint.z > 0 &&
                          viewportPoint.x >= 0f && viewportPoint.x <= 1f &&
                          viewportPoint.y >= 0f && viewportPoint.y <= 1f;

        // Check peripheral FOV
        Vector3 dir = (transform.position - playerCam.transform.position).normalized;
        float angle = Vector3.Angle(playerCam.transform.forward, dir);
        bool inPeripheral = angle < peripheralAngle;

        // Raycast to check line-of-sight (no wall in between)
        bool losClear = false;
        if (Physics.Raycast(playerCam.transform.position + Vector3.up, dir, out RaycastHit hit, sightRadius))
        {
            losClear = hit.collider.gameObject == gameObject;
        }

        return (inViewport || inPeripheral) && losClear;
    }
}