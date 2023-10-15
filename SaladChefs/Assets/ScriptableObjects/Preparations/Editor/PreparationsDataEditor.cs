using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PreparationsDataEditor : EditorWindow
{
    const string kMenuDataFileName = "Assets/ScriptableObjects/Preparations/MenuData.asset";
    const string kIngredientFileLocation = "Assets/ScriptableObjects/Preparations/Ingredients/";

    IngredientData ingredientData;
    MenuData menuData;
    Salad salad;
    Ingredient ingredient;

    [MenuItem("Salad Chef/Salads/Salad Editor")]
    static void Init()
    {
        GetWindow<PreparationsDataEditor>();
    }

    private void OnEnable()
    {
        menuData = AssetDatabase.LoadAssetAtPath(kMenuDataFileName, typeof(MenuData)) as MenuData;
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Ingredient");

        ingredient = (Ingredient)EditorGUILayout.EnumPopup("Ingredient : ", ingredient);

        if (GUILayout.Button("Create New Ingredient Data"))
        {
            ingredientData = CreateInstance<IngredientData>();
            ingredientData.IngredientName = ingredient;
            AssetDatabase.CreateAsset(ingredientData, string.Concat(kIngredientFileLocation, ingredient.ToString(), ".asset"));
            AssetDatabase.SaveAssets();
        }

        GUILayout.Label("Create Menu");

        if (GUILayout.Button("Create New Menu Data"))
        {
            menuData = CreateInstance<MenuData>();
            menuData.Salads = new List<SaladData>();
            AssetDatabase.CreateAsset(menuData, kMenuDataFileName);
            AssetDatabase.SaveAssets();
        }

        GUILayout.Label("Add Salad");

        salad = (Salad)EditorGUILayout.EnumPopup("Salad : ", salad);

        if (GUILayout.Button("Add New Salad"))
        {
            int saladIndex = (int)salad;
            SaladData saladData = CreateInstance<SaladData>();
            saladData.SaladName = salad;
            saladData.name = salad.ToString();
            menuData.Salads.Add(saladData);
            AssetDatabase.AddObjectToAsset(saladData, menuData);
            AssetDatabase.SaveAssets();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(ingredientData);
            EditorUtility.SetDirty(menuData);
        }

        Repaint();
    }
}
