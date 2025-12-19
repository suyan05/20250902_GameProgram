using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ItemDamageModifier
{
    public ItemType itemType;
    public int extraDamage;
}

public class DamageModifier : MonoBehaviour
{
    public static DamageModifier Instance;

    [Header("ItemType별 추가 데미지 설정")]
    public ItemDamageModifier[] modifiers;

    private Dictionary<ItemType, int> modifierDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            modifierDict = new Dictionary<ItemType, int>();
            foreach (var m in modifiers)
                modifierDict[m.itemType] = m.extraDamage;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetExtraDamage(ItemType type)
    {
        return modifierDict.TryGetValue(type, out int dmg) ? dmg : 0;
    }
}