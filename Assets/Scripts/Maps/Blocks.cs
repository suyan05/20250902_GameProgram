using UnityEngine;
using System.Collections.Generic;

public enum BlockType
{
    Dirt = 0,
    Grass = 1,
    Water = 2,
    Stone = 3,
    Ore = 4,
    Empty = 99 // 인벤토리용으로 따로 분리
}
[System.Serializable]
public struct DropItem
{
    public BlockType type;
    public int count;
    [Range(0f, 1f)] public float dropChance;
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

    [Header("Optional Effects")]
    public GameObject breakEffect;

    private void Awake()
    {
        hp = maxHP;

        transform.position += new Vector3(0, 1f, 0);

        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();

        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
            gameObject.tag = "Block";

        if (drops.Count == 0)
        {
            drops.Add(new DropItem { type = type, count = 1, dropChance = 1f });
        }
    }

    public void Hit(int damage)
    {
        if (!mineable) return;

        hp -= damage;

        if (hp <= 0)
        {
            DropToInventory();
            Break();
        }
    }

    private void DropToInventory()
    {
        foreach (var item in drops)
        {
            if (Random.value <= item.dropChance)
            {
                InventoryManager.Instance.Add(item.type, item.count);
            }
        }
    }

    private void Break()
    {
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}