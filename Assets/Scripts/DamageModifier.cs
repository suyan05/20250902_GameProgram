using UnityEngine;

[System.Serializable]
public struct ItemDamageModifier
{
    public ItemType itemType;   // 어떤 아이템인지
    public int extraDamage;     // 추가 데미지
}

public class DamageModifier : MonoBehaviour
{
    public static DamageModifier Instance;

    [Header("ItemType별 추가 데미지 설정")]
    public ItemDamageModifier[] modifiers; // 인스펙터에서 수정 가능

    private void Awake()
    {
        Instance = this;
    }
    public int GetExtraDamage(ItemType type)
    {
        foreach (var m in modifiers)
        {
            if (m.itemType == type)
                return m.extraDamage;
        }
        return 0;
    }
}