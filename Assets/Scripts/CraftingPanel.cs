using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    [Header("UI References")]
    public GameObject craftingUI;
    public Transform recipeListParent;
    public GameObject recipeButtonPrefab;
    public TextMeshProUGUI messageText;

    [Header("Recipes")]
    public List<CraftingRecipeSO> recipes;

    private void Start()
    {
        craftingUI.SetActive(false);
        PopulateRecipeList();
        messageText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isActive = !craftingUI.activeSelf;
            craftingUI.SetActive(isActive);

            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isActive;
        }
    }

    /// <summary>
    /// 레시피 버튼 생성
    /// </summary>
    private void PopulateRecipeList()
    {
        foreach (var recipe in recipes)
        {
            GameObject buttonObj = Instantiate(recipeButtonPrefab, recipeListParent);
            var btn = buttonObj.GetComponent<Button>();
            var txt = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            // 버튼 텍스트: 결과 아이템 이름 + 재료 목록
            string recipeText = recipe.displayName + " (";
            for (int i = 0; i < recipe.inputs.Count; i++)
            {
                var ing = recipe.inputs[i];
                recipeText += ing.ItemType + " x" + ing.count;
                if (i < recipe.inputs.Count - 1) recipeText += ", ";
            }
            recipeText += ")";
            txt.text = recipeText;

            btn.onClick.AddListener(() => TryCraft(recipe));
        }
    }

    /// <summary>
    /// 조합 시도
    /// </summary>
    private void TryCraft(CraftingRecipeSO recipe)
    {
        // 재료 확인
        foreach (var ingredient in recipe.inputs)
        {
            if (!HasEnoughItem(ingredient.ItemType, ingredient.count))
            {
                messageText.text = "재료가 부족합니다!";
                return;
            }
        }

        // 재료 소모
        foreach (var ingredient in recipe.inputs)
        {
            RemoveItem(ingredient.ItemType, ingredient.count);
        }

        // 결과 아이템 추가
        foreach (var product in recipe.OutPut)
        {
            InventoryManager.Instance.Add(product.ItemType, product.count);
        }

        messageText.text = recipe.displayName + " 제작 성공!";
    }

    private bool HasEnoughItem(ItemType type, int count)
    {
        int totalCount = 0;
        foreach (var slot in InventoryManager.Instance.hotbarSlots)
            if (slot.type == type) totalCount += slot.count;
        foreach (var slot in InventoryManager.Instance.inventorySlots)
            if (slot.type == type) totalCount += slot.count;

        return totalCount >= count;
    }

    private void RemoveItem(ItemType type, int count)
    {
        count = RemoveFromSlots(InventoryManager.Instance.hotbarSlots, type, count);
        if (count > 0)
            RemoveFromSlots(InventoryManager.Instance.inventorySlots, type, count);
    }

    private int RemoveFromSlots(InventorySlot[] slots, ItemType type, int count)
    {
        foreach (var slot in slots)
        {
            if (slot.type == type && count > 0)
            {
                int removeAmount = Mathf.Min(slot.count, count);
                slot.count -= removeAmount;
                count -= removeAmount;

                if (slot.count <= 0) slot.Clear();
                else
                {
                    slot.countText.text = slot.count > 1 ? slot.count.ToString() : "";
                    slot.countText.enabled = slot.count > 1;
                }
            }
        }
        return count;
    }
}