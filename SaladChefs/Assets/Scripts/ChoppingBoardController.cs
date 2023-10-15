using UnityEngine;

public class ChoppingBoardController : MonoBehaviour
{
    public GameObject[] choppingIngredients; // in the order of Ingredient Enum

    int _currentIngredientIndex;

    void Start()
    {
        _currentIngredientIndex = -1;
    }

    public IngredientData Add(Ingredient rawIngredient)
    {
        if(_currentIngredientIndex > -1)
        {
            choppingIngredients[_currentIngredientIndex].SetActive(false);
        }
        _currentIngredientIndex = (int)rawIngredient;
        choppingIngredients[_currentIngredientIndex].SetActive(true);
        return choppingIngredients[_currentIngredientIndex].GetComponent<IngredientInfo>().ingredientData;
    }

    public void Remove()
    {
        if (_currentIngredientIndex > -1)
        {
            choppingIngredients[_currentIngredientIndex].SetActive(false);
            _currentIngredientIndex = -1;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < choppingIngredients.Length; i++)
        {
            choppingIngredients[i].SetActive(false);
        }
    }
}
