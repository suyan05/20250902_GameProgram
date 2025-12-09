using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "조합법 생성")]
public class CraftingRecipeSO : ScriptableObject
{
    [Serializable] public struct Ingredient
    {
        public ItemType ItemType;
        public int count;
    }

    [Serializable]public struct Product
    {
        public ItemType ItemType;
        public int count;
    }

    public string displayName;
    public List<Ingredient> inputs = new();
    public List<Product> OutPut = new();
}
