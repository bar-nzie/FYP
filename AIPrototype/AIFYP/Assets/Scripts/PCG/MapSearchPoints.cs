using System.Collections.Generic;
using UnityEngine;

public class MapSearchPoints : MonoBehaviour
{
    public List<Vector3> searchPoints = new();

    public void GeneratePoints(float tileSizeX, float tileSizeZ, int width, int depth)
    {
        searchPoints.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float posX = transform.position.x + x * tileSizeX;
                float posZ = transform.position.z + z * tileSizeZ;
                float posY = 0f;

                // Raycast down from high above to find the actual ground
                if (Physics.Raycast(new Vector3(posX, 200f, posZ), Vector3.down, out RaycastHit hit, 500f))
                {
                    posY = hit.point.y + 0.5f; // add a small offset so AI isn't inside terrain
                    searchPoints.Add(new Vector3(posX, posY, posZ));
                    Debug.Log("Added search point: " + new Vector3(posX, posY, posZ));
                }
            }
        }
    }
}