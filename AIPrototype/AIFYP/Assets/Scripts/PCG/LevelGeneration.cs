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
    void Start()
    {
        GenerateMap();

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);

        navMeshSurface.BuildNavMesh();

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
        Instantiate(enemy, new Vector3(115, 10, 115), Quaternion.identity);
    }
}
