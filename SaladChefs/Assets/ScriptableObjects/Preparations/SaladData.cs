using UnityEngine;

public class SaladData : ScriptableObject
{
    [SerializeField]
    Salad saladName;

    [SerializeField]
    IngredientData[] ingredients;

    public Salad SaladName
    {
        get
        {
            return saladName;
        }
        set
        {
            saladName = value;
        }
    }

    public IngredientData[] Ingredients
    {
        get
        {
            return ingredients;
        }
    }

    public SaladData()
    {

    }

    public SaladData(Salad salad)
    {
        saladName = salad;
    }
}
