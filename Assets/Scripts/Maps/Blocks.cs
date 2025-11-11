using UnityEngine;
using System.Collections.Generic;

public enum BlockType { Dirt, Grass, Water, Stone, Ore }

[System.Serializable]
public struct DropItem
{
    public BlockType type;
    public int count;
    [Range(0f, 1f)] public float dropChance; // 0.0 ~ 1.0 사이 확률
}

public class Blocks : MonoBehaviour
{
    [Header("Block Stat")]
    public BlockType type = BlockType.Dirt;
    public int maxHP = 3;
    [HideInInspector] public int hp;
    public bool mineable = true;

    [Header("Drop Items")]
    public List<DropItem> drops = new List<DropItem>();

    private void Awake()
    {
        hp = maxHP;
        if (GetComponent<Collider>() == null) gameObject.AddComponent<BoxCollider>();
        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
            gameObject.tag = "Block";

        // 기본 드롭 설정
        if (drops.Count == 0)
        {
            drops.Add(new DropItem { type = type, count = 1, dropChance = 1f });
        }
    }

    public void Hit(int damage, Inventory inven)
    {
        if (!mineable) return;

        hp -= damage;

        if (hp <= 0)
        {
            if (inven != null)
            {
                foreach (var item in drops)
                {
                    if (Random.value <= item.dropChance)
                    {
                        inven.Add(item.type, item.count);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}