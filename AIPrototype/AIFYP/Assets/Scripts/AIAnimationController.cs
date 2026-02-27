using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AIAnimationController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Settings")]
    public float rotationSpeed = 5f; // how fast the character rotates toward movement
    public float idleThreshold = 0.1f; // velocity magnitude below which AI is idle

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovementAnimation();
        //HandleRotation();
    }

    void HandleMovementAnimation()
    {
        // Use agent velocity to determine speed
        float speed = agent.velocity.magnitude;
        float normalizedSpeed = Mathf.Clamp01(speed / agent.speed); // 0 = idle, 1 = full speed

        animator.SetFloat("MoveSpeed", normalizedSpeed, 0.1f, Time.deltaTime); // smooth blend

    }

    void HandleRotation()
    {
        Vector3 velocity = agent.velocity;
        velocity.y = 0f; // ignore vertical rotation

        if (velocity.sqrMagnitude > 0.001f)
        {
            // Compute the signed angle between forward and movement
            float angle = Vector3.SignedAngle(transform.forward, velocity.normalized, Vector3.up);

            // Determine turn direction: left (-1), right (1), or none (0)
            int turnDirection = 0;
            if (Mathf.Abs(angle) > 5f) // small threshold to ignore tiny jitter
            {
                turnDirection = angle > 0 ? 1 : -1;
            }

            animator.SetInteger("TurnDirection", turnDirection);

            // Rotate smoothly toward movement
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Stop turning if not moving
            animator.SetInteger("TurnDirection", 0);
        }
    }
}