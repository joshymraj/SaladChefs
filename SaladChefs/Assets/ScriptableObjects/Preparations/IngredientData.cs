using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientData : ScriptableObject
{
    [SerializeField]
    Ingredient ingredientName;

    [SerializeField]
    Sprite image;

    [SerializeField]
    float chopTime;

    [SerializeField]
    float pickupCartItemWidth;

    [SerializeField]
    float pickupCartItemHeight;

    public Ingredient IngredientName
    {
        get
        {
            return ingredientName;
        }
        set
        {
            ingredientName = value;
        }
    }

    public Sprite Image
    {
        get
        {
            return image;
        }
    }

    public float ChopTime
    {
        get
        {
            return chopTime;
        }
    }

    public float PickupCartItemWidth
    {
        get
        {
            return pickupCartItemWidth;
        }
    }

    public float PickupCartItemHeight
    {
        get
        {
            return pickupCartItemHeight;
        }
    }
}
