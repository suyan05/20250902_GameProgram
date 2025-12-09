using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text countText;

    public ItemType type = ItemType.Empty;
    public int count = 0;

    public void SetItem(ItemType newType, int newCount)
    {
        type = newType;
        count = newCount;

        icon.sprite = InventoryManager.Instance.GetIcon(type);
        icon.enabled = true;

        countText.text = count > 1 ? count.ToString() : "";
        countText.enabled = count > 1;

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        type = ItemType.Empty;
        count = 0;

        icon.sprite = null;
        icon.enabled = false;

        countText.text = "";
        countText.enabled = false;

        gameObject.SetActive(false);
    }
}