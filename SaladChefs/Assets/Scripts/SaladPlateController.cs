using UnityEngine;

public class SaladPlateController : MonoBehaviour
{
    public Material[] saladMaterials;
    public Material saladPlateMaterial;
    public Material saladPlateBorderMaterial;

    [HideInInspector]
    public int chefIndex; // to be set by chef controller

    MeshRenderer saladPlateRenderer;

    public Ingredient[] Ingredients
    {
        get;
        private set;
    }

    public int NoOfIngredientsInPlate
    {
        get;
        private set;
    }

    void Start()
    {
        saladPlateRenderer = GetComponent<MeshRenderer>();
    }

    void UpdateSaladPlate()
    {
        Material[] newSaladPlateMaterials = new Material[3];
        newSaladPlateMaterials[0] = saladPlateMaterial;
        newSaladPlateMaterials[1] = saladPlateBorderMaterial;
        newSaladPlateMaterials[2] = saladMaterials[Random.Range(0, saladMaterials.Length)];
        saladPlateRenderer.materials = newSaladPlateMaterials;
    }

    public void Add(Ingredient choppedIngredient)
    {
        Ingredient[] newIngredientList = new Ingredient[NoOfIngredientsInPlate + 1];
        if(NoOfIngredientsInPlate > 0)
        {
            for (int i = 0; i < NoOfIngredientsInPlate; i++)
            {
                newIngredientList[i] = Ingredients[i];
            }
            NoOfIngredientsInPlate += 1;
            newIngredientList[NoOfIngredientsInPlate - 1] = choppedIngredient;
        }
        else
        {
            NoOfIngredientsInPlate = 1;
            newIngredientList[0] = choppedIngredient;
        }

        UpdateSaladPlate();
        Ingredients = newIngredientList;
    }

    public void Clear()
    {
        Ingredients = null;
        Material[] newSaladPlateMaterials = new Material[2];
        newSaladPlateMaterials[0] = saladPlateMaterial;
        newSaladPlateMaterials[1] = saladPlateBorderMaterial;
        saladPlateRenderer.materials = newSaladPlateMaterials;
        NoOfIngredientsInPlate = 0;
    }
}
