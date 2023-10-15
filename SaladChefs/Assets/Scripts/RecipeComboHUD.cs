using UnityEngine;
using UnityEngine.UI;

public class RecipeComboHUD : MonoBehaviour
{
    public Image[] recipeIngredientImages;

    public void Show(IngredientData[] ingredients)
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            recipeIngredientImages[i].sprite = ingredients[i].Image;
        }
    }
}
