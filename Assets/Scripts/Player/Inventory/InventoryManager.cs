using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventorySlot[] hotbarSlots;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryUI;
    public Sprite[] blockIcons;

    private void Awake()
    {
        Instance = this;
        inventoryUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    public Sprite GetIcon(BlockType type)
    {
        return blockIcons[(int)type];
    }

    public void Add(BlockType type, int count)
    {
        if (TryAddToSlots(hotbarSlots, type, count)) return;
        TryAddToSlots(inventorySlots, type, count);
    }

    private bool TryAddToSlots(InventorySlot[] slots, BlockType type, int count)
    {
        // 1. 같은 타입이면 수량만 증가
        foreach (var slot in slots)
        {
            if (slot.gameObject.activeSelf && slot.type == type)
            {
                slot.count += count;
                slot.countText.text = slot.count > 1 ? slot.count.ToString() : "";
                slot.countText.enabled = slot.count > 1;
                return true;
            }
        }

        // 2. 비어 있는 슬롯만 새로 채움
        foreach (var slot in slots)
        {
            if (!slot.gameObject.activeSelf || slot.type == BlockType.Empty)
            {
                slot.SetItem(type, count);
                return true;
            }
        }

        // 3. 다른 타입이 이미 들어있는 슬롯은 무시
        return false;
    }
}