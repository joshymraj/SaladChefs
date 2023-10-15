using UnityEngine;

public class ChoppingPlateController : MonoBehaviour
{
    public GameObject[] rawIngredients; // in the order of Ingredient Enum

    CustomQueue<Ingredient> _rawIngredients;

    public bool HasIngredientsInChoppingPlate
    {
        get
        {
            return _rawIngredients.Size > 0;
        }
    }

    void Start()
    {
        _rawIngredients = new CustomQueue<Ingredient>();
    }

    public void Add(Ingredient rawIngredient)
    {
        _rawIngredients.Enqueue(rawIngredient);
        rawIngredients[(int)rawIngredient].SetActive(true);
    }

    public Ingredient Take()
    {
        Ingredient ingredientToChop = _rawIngredients.Dequeue();
        rawIngredients[(int)ingredientToChop].SetActive(false);

        if(_rawIngredients.Size == 0)
        {
            _rawIngredients = new CustomQueue<Ingredient>();
        }

        return ingredientToChop;
    }

    public void Clear()
    {
        _rawIngredients = null;
        for (int i = 0; i < rawIngredients.Length; i++)
        {
            rawIngredients[i].SetActive(false);
        }
    }
}
