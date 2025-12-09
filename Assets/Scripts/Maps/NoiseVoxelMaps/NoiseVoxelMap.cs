using System.Collections.Generic;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public static NoiseVoxelMap Instance;

    public GameObject[] blockPrefabs;
    // 0: Dirt, 1: Grass, 2: Water, 3: Stone, 4: Ore

    public int width = 20;
    public int maxHeight = 16;
    public int depth = 20;

    [SerializeField] float noiseScale = 20f;
    [SerializeField] int waterLevelMin = 2;
    [SerializeField] int waterLevelMax = 4;
    [SerializeField] float caveNoiseScale = 10f;
    [SerializeField] float oreProbability = 0.1f;

    private int[,] heightMap;

    private void Awake()
    {
        Instance = this;   //ΩÃ±€≈Ê √ ±‚»≠
    }

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

                Place(1, x, h, z); // Grass

                for (int y = h - 1; y >= Mathf.Max(h - 5, 0); y--)
                    Place(0, x, y, z); // Dirt

                for (int y = h - 6; y >= 0; y--)
                {
                    float caveNoise = Mathf.PerlinNoise(
                        (x + caveOffsetX) / caveNoiseScale + y * 0.1f,
                        (z + caveOffsetZ) / caveNoiseScale + y * 0.1f
                    );

                    if (caveNoise > 0.5f) continue;

                    int index = Random.value < oreProbability ? 4 : 3;
                    Place(index, x, y, z); // Ore or Stone
                }

                for (int y = h + 1; y <= waterLevelMax; y++)
                {
                    if (y >= waterLevelMin && y <= waterLevelMax)
                        Place(2, x, y, z); // Water
                }
            }
        }
    }

    public void Place(int index, int x, int y, int z)
    {
        if (index < 0 || index >= blockPrefabs.Length) return;

        GameObject prefab = blockPrefabs[index];
        var go = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"{prefab.name}_{x}_{y}_{z}";

        var b = go.GetComponent<Blocks>() ?? go.AddComponent<Blocks>();
        b.type = (ItemType)index;
        b.maxHP = GetHPByType(b.type);
        b.mineable = b.type != ItemType.Water;

        b.drops = new List<DropItem>
        {
            new DropItem { type = b.type, count = 1, dropChance = 1f }
        };

        if (b.type == ItemType.Stone)
        {
            b.drops.Add(new DropItem { type = ItemType.Dirt, count = 2, dropChance = 0.5f });
        }
    }

    private int GetHPByType(ItemType type)
    {
        return type switch
        {
            ItemType.Dirt => 3,
            ItemType.Grass => 2,
            ItemType.Stone => 5,
            ItemType.Ore => 6,
            ItemType.Water => 1,
            _ => 3
        };
    }
}