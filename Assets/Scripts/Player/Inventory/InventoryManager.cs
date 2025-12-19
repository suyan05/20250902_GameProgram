using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventorySlot[] hotbarSlots;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryUI;
    public Sprite[] blockIcons;

    public GameObject[] blockPrefabs;

    public int selectedHotbarIndex = 0; // 현재 선택된 슬롯

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inventoryUI.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 인벤토리 열기/닫기
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = !inventoryUI.activeSelf;
            inventoryUI.SetActive(isActive);

            var harvester = FindObjectOfType<PlayerHarvester>();
            if (harvester != null)
            {
                harvester.SetBuildMode(!isActive);
            }

            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isActive;
        }

        // 숫자키로 핫바 선택
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectHotbar(i);
            }
        }

        // 마우스 휠로 핫바 이동
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            selectedHotbarIndex = (selectedHotbarIndex + 1) % hotbarSlots.Length;
            SelectHotbar(selectedHotbarIndex);
        }
        else if (scroll < 0f)
        {
            selectedHotbarIndex = (selectedHotbarIndex - 1 + hotbarSlots.Length) % hotbarSlots.Length;
            SelectHotbar(selectedHotbarIndex);
        }
    }

    private void SelectHotbar(int index)
    {
        selectedHotbarIndex = index;
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i].GetComponent<Image>().color =
                (i == selectedHotbarIndex) ? Color.yellow : Color.white;
        }
    }

    public Sprite GetIcon(ItemType type)
    {
        if ((int)type < 0 || (int)type >= blockIcons.Length) return null;
        return blockIcons[(int)type];
    }

    /// <summary>
    /// 아이템 추가 (먼저 핫바 → 인벤토리)
    /// </summary>
    public void Add(ItemType type, int count)
    {
        if (TryAddToSlots(hotbarSlots, type, count)) return;
        TryAddToSlots(inventorySlots, type, count);
    }

    private bool TryAddToSlots(InventorySlot[] slots, ItemType type, int count)
    {
        // 이미 같은 아이템이 있는 슬롯 → 개수 증가
        foreach (var slot in slots)
        {
            if (slot.type == type && slot.count > 0)
            {
                slot.SetItem(type, slot.count + count);
                return true;
            }
        }

        // 빈 슬롯에 새로 추가
        foreach (var slot in slots)
        {
            if (slot.type == ItemType.Empty || slot.count <= 0)
            {
                slot.SetItem(type, count);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 아이템 소모 (핫바 → 인벤토리 순서)
    /// </summary>
    public void Remove(ItemType type, int count)
    {
        count = RemoveFromSlots(hotbarSlots, type, count);
        if (count > 0)
            RemoveFromSlots(inventorySlots, type, count);
    }

    private int RemoveFromSlots(InventorySlot[] slots, ItemType type, int count)
    {
        foreach (var slot in slots)
        {
            if (slot.type == type && slot.count > 0 && count > 0)
            {
                int removeAmount = Mathf.Min(slot.count, count);
                int newCount = slot.count - removeAmount;
                count -= removeAmount;

                if (newCount <= 0) slot.Clear();
                else slot.SetItem(type, newCount);
            }
        }
        return count;
    }
}