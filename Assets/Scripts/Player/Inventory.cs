using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Image icon;
        public Text countText;
        public BlockType type;
        public int count;
        public bool isOccupied => count > 0;
    }

    public GameObject inventoryPanel; // I 키로 열고 닫을 패널
    public List<Slot> slots = new List<Slot>();

    private Dictionary<BlockType, int> totalCounts = new Dictionary<BlockType, int>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    public void Add(BlockType type, int count)
    {
        if (!totalCounts.ContainsKey(type))
            totalCounts[type] = 0;

        totalCounts[type] += count;

        // 이미 있는 슬롯에 추가
        foreach (var slot in slots)
        {
            if (slot.isOccupied && slot.type == type)
            {
                slot.count += count;
                slot.countText.text = slot.count.ToString();
                return;
            }
        }

        // 빈 슬롯에 새로 추가
        foreach (var slot in slots)
        {
            if (!slot.isOccupied)
            {
                slot.type = type;
                slot.count = count;
                slot.icon.sprite = GetBlockIcon(type); // 아이콘 설정
                slot.icon.enabled = true;
                slot.countText.text = count.ToString();
                return;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
    }

    private Sprite GetBlockIcon(BlockType type)
    {
        // 아이콘 스프라이트를 반환하는 로직 (Resources.Load 등)
        return Resources.Load<Sprite>($"Icons/{type}");
    }
}