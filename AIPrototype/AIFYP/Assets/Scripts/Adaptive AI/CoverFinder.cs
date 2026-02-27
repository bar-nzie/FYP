using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CoverFinder : MonoBehaviour
{
    public float sampleRadius = 10f;
    public int sampleCount = 20;

    /// <summary>
    /// Returns a cover point near the AI that blocks line of sight from the player.
    /// </summary>
    public Vector3 FindCoverPoint(Vector3 aiPos, Vector3 playerPos)
    {
        List<Vector3> candidates = new List<Vector3>();

        for (int i = 0; i < sampleCount; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * sampleRadius;
            Vector3 sample = aiPos + new Vector3(randomCircle.x, 0f, randomCircle.y);

            // Ensure the point is on the NavMesh
            if (NavMesh.SamplePosition(sample, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                // Check if line of sight is blocked by any collider
                if (Physics.Linecast(playerPos, hit.position, out RaycastHit _))
                {
                    candidates.Add(hit.position);
                }
            }
        }

        if (candidates.Count > 0)
        {
            // Pick a random valid cover point
            return candidates[Random.Range(0, candidates.Count)];
        }

        // Fallback: just move a short distance away from player
        Vector3 fallback = aiPos + (aiPos - playerPos).normalized * (sampleRadius * 0.5f);
        if (NavMesh.SamplePosition(fallback, out NavMeshHit fallbackHit, 1.0f, NavMesh.AllAreas))
            return fallbackHit.position;

        return aiPos; // stay in place if no valid point
    }
}