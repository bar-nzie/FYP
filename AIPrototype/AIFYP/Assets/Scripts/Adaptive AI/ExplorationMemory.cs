using System.Collections.Generic;
using UnityEngine;

public class ExplorationMemory : MonoBehaviour
{
    public float minDistance = 1f;      // distance to consider a point already visited
    public float searchRadius = 10f;    // radius to pick next search point near current position

    private List<Vector3> visitedPoints = new();

    /// <summary>
    /// Returns the next unvisited search point within searchRadius of currentPosition.
    /// Returns Vector3.zero if no unvisited points are available.
    /// </summary>
    public Vector3 GetNextSearchPoint(List<Vector3> potentialPoints, Vector3 currentPosition)
    {
        List<Vector3> unvisitedNearby = new List<Vector3>();

        foreach (var point in potentialPoints)
        {
            float dist = Vector3.Distance(currentPosition, point);
            if (dist <= searchRadius)
            {
                bool visited = false;
                foreach (var v in visitedPoints)
                {
                    if (Vector3.Distance(v, point) < minDistance)
                    {
                        visited = true;
                        break;
                    }
                }

                if (!visited)
                    unvisitedNearby.Add(point);
            }
        }

        if (unvisitedNearby.Count > 0)
        {
            Vector3 chosen = unvisitedNearby[Random.Range(0, unvisitedNearby.Count)];
            visitedPoints.Add(chosen);
            return chosen;
        }

        List<Vector3> unvisitedGlobal = new List<Vector3>();
        foreach (var point in potentialPoints)
        {
            bool visited = false;
            foreach (var v in visitedPoints)
            {
                if (Vector3.Distance(v, point) < minDistance)
                {
                    visited = true;
                    break;
                }
            }

            if (!visited)
                unvisitedGlobal.Add(point);
        }

        // Pick a random unvisited global point
        if (unvisitedGlobal.Count > 0)
        {
            Vector3 chosen = unvisitedGlobal[Random.Range(0, unvisitedGlobal.Count)];
            visitedPoints.Add(chosen);
            return chosen;
        }

        return Vector3.zero;
    }

    public void ResetMemory()
    {
        visitedPoints.Clear();
    }
}