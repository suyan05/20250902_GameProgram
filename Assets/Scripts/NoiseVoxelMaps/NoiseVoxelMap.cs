using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public GameObject[] blockPrefabs;
    // 0: Dirt, 1: Grass, 2: Water, 3: Stone, 4: Ore

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public int width = 20;
    public int maxHeight = 16;
    public int depth = 20;

    [SerializeField] float noiseScale = 20f;
    [SerializeField] int waterLevelMin = 2;
    [SerializeField] int waterLevelMax = 4;
    [SerializeField] float caveNoiseScale = 10f;
    [SerializeField] float oreProbability = 0.1f;

    private int[,] heightMap;

    private void Start()
    {
        heightMap = new int[width, depth];

        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);
        float caveOffsetX = Random.Range(-9999f, 9999f);
        float caveOffsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;

                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.FloorToInt(noise * maxHeight);
                heightMap[x, z] = h;

                Place(blockPrefabs[1], x, h, z); // Grass

                for (int y = h - 1; y >= Mathf.Max(h - 5, 0); y--)
                    Place(blockPrefabs[0], x, y, z); // Dirt

                for (int y = h - 6; y >= 0; y--)
                {
                    float caveNoise = Mathf.PerlinNoise(
                        (x + caveOffsetX) / caveNoiseScale + y * 0.1f,
                        (z + caveOffsetZ) / caveNoiseScale + y * 0.1f
                    );

                    if (caveNoise > 0.5f) continue;

                    GameObject prefab = Random.value < oreProbability ? blockPrefabs[4] : blockPrefabs[3];
                    Place(prefab, x, y, z); // Ore or Stone
                }

                // 물 생성 조건
                for (int y = h + 1; y <= waterLevelMax; y++)
                {
                    if (y >= waterLevelMin && y <= waterLevelMax)
                        Place(blockPrefabs[2], x, y, z); // Water
                }
            }
        }

        //SpawnPlayer();
        //SpawnEnemies();
    }

    private void Place(GameObject prefab, int x, int y, int z)
    {
        var go = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"{prefab.name}_{x}_{y}_{z}";
    }

    private void SpawnPlayer()
    {
        int midX = width / 2;
        int midZ = depth / 2;
        int h = heightMap[midX, midZ];

        Vector3 spawnPos = new Vector3(midX, h + 1, midZ);
        Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    }

    private void SpawnEnemies()
    {
        for (int x = 2; x < width; x += 5)
        {
            for (int z = 2; z < depth; z += 5)
            {
                int h = heightMap[x, z];
                Vector3 pos = new Vector3(x, h + 1, z);
                Instantiate(enemyPrefab, pos, Quaternion.identity);
            }
        }
    }
}