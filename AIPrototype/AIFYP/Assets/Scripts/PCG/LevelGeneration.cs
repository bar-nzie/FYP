using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    private int mapWidthInTiles, mapDepthInTiles;
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private NavMeshSurface navMeshSurface;
    public GameObject enemy;

    public MapSearchPoints mapSearchPoints;

    void Start()
    {
        GenerateMap();

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);

        navMeshSurface.BuildNavMesh();
        if (mapSearchPoints != null)
        {
            Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
            mapSearchPoints.GeneratePoints(tileSize.x, tileSize.z, mapWidthInTiles, mapDepthInTiles);
        }

        SpawnEnemies();

    }

    void GenerateMap()
    {
        // get the tile dimensions from the tile Prefab
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;
        // for each Tile, instantiate a Tile in the correct position
        for (int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++)
        {
            for (int zTileIndex = 0; zTileIndex < mapDepthInTiles; zTileIndex++)
            {
                // calculate the tile position based on the X and Z indices
                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileWidth,
                  this.gameObject.transform.position.y,
                  this.gameObject.transform.position.z + zTileIndex * tileDepth);
                // instantiate a new Tile
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
            }
        }
    }

    void SpawnEnemies()
    {
        int enemyCount = 20;

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 randomPosition = GetRandomPointOnMap();

            Instantiate(enemy, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointOnMap()
    {
        float mapWidth = mapWidthInTiles * tilePrefab.GetComponent<MeshRenderer>().bounds.size.x;
        float mapDepth = mapDepthInTiles * tilePrefab.GetComponent<MeshRenderer>().bounds.size.z;

        float randomX = Random.Range(transform.position.x, transform.position.x + mapWidth);
        float randomZ = Random.Range(transform.position.z, transform.position.z + mapDepth);

        Vector3 rayStart = new Vector3(randomX, 200f, randomZ); // start high above map

        RaycastHit hit;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 500f))
        {
            Debug.Log("Spawn point found at: " + hit.point);
            return hit.point + Vector3.up * 1f; // spawn slightly above ground
        }

        // fallback (shouldn't happen)
        return new Vector3(randomX, 10f, randomZ);
    }
}
