using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class TacticalPositionFinder : MonoBehaviour
{
    public float searchRadius = 20f;
    public int samplePoints = 20;

    public Vector3 FindBestPosition(Vector3 aiPos, Transform player)
    {
        Vector3 bestPoint = aiPos;
        float bestScore = float.MinValue;

        for (int i = 0; i < samplePoints; i++)
        {
            Vector3 random = aiPos + Random.insideUnitSphere * searchRadius;
            random.y = aiPos.y;

            if (NavMesh.SamplePosition(random, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                float score = EvaluatePosition(hit.position, player);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestPoint = hit.position;
                }
            }
        }

        return bestPoint;
    }

    float EvaluatePosition(Vector3 position, Transform player)
    {
        float score = 0;

        score += HeightScore(position, player);
        score += VisibilityScore(position, player);
        score += ChokepointScore(position);

        return score;
    }

    float HeightScore(Vector3 pos, Transform player)
    {
        float heightDiff = pos.y - player.position.y;
        return heightDiff * 2f; 
    }

    float VisibilityScore(Vector3 pos, Transform player)
    {
        Vector3 dir = player.position - pos;

        if (Physics.Raycast(pos, dir.normalized, out RaycastHit hit, dir.magnitude))
        {
            if (hit.transform != player)
                return 5f; 
        }

        return -5f; 
    }

    float ChokepointScore(Vector3 pos)
    {
        int nearbyObstacles = Physics.OverlapSphere(pos, 2f).Length;

        return nearbyObstacles * 0.5f; 
    }
}