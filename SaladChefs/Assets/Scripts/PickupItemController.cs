using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupItemController : MonoBehaviour
{
    public Ingredient Ingredient
    {
        get;
        private set;
    }

    public void SetItem(Sprite ingredientImage, Ingredient ingredient)
    {
        gameObject.GetComponent<Image>().sprite = ingredientImage;
        Ingredient = ingredient;
    }
}
