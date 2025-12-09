using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<ItemType, int> items = new();

    public InventorySlot[] hotbarSlots;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryUI;
    public Sprite[] blockIcons;

    public GameObject[] blockPrefabs;

    public int selectedHotbarIndex = 0; // 현재 선택된 슬롯

    private void Awake()
    {
        Instance = this;
        inventoryUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = !inventoryUI.activeSelf;
            inventoryUI.SetActive(isActive);

            // 인벤토리 열면 설치모드 해제, 닫으면 다시 활성화
            var harvester = FindObjectOfType<PlayerHarvester>();
            if (harvester != null)
            {
                harvester.SetBuildMode(!isActive);
            }

            // 마우스 고정/해제
            if (isActive)
            {
                Cursor.lockState = CursorLockMode.None;   // 마우스 자유
                Cursor.visible = true;                   // 커서 보이기
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // 마우스 고정
                Cursor.visible = false;                  // 커서 숨기기
            }
        }

        //숫자패드로 슬롯 선택 (KeyCode.Alpha1 ~ Alpha9)
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectHotbar(i);
            }
        }

        //마우스 휠로 슬롯 이동
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
        //UI 강조 효과 (예: 선택된 슬롯 테두리 색 변경)
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i].GetComponent<UnityEngine.UI.Image>().color =
                (i == selectedHotbarIndex) ? Color.yellow : Color.white;
        }
    }

    public Sprite GetIcon(ItemType type)
    {
        return blockIcons[(int)type];
    }

    public void Add(ItemType type, int count)
    {
        if (TryAddToSlots(hotbarSlots, type, count)) return;
        TryAddToSlots(inventorySlots, type, count);
    }

    private bool TryAddToSlots(InventorySlot[] slots, ItemType type, int count)
    {
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

        foreach (var slot in slots)
        {
            if (!slot.gameObject.activeSelf || slot.type == ItemType.Empty)
            {
                slot.SetItem(type, count);
                return true;
            }
        }

        return false;
    }
}