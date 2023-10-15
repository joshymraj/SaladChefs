using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class PowerUpSpawnAreaEditor : EditorWindow
{
    const string kSpawnAreaDataFileName = "Assets/ScriptableObjects/PowerUps/PowerUpSpawnData.asset";

    PowerUpSpawnData powerUpSpawnData;

    string newSpawnAreaName;

    [MenuItem("Salad Chef/Power Ups/Spawn Area Editor")]
    static void Init()
    {
        GetWindow<PowerUpSpawnAreaEditor>();
    }

    private void OnEnable()
    {
        powerUpSpawnData = AssetDatabase.LoadAssetAtPath(kSpawnAreaDataFileName, typeof(PowerUpSpawnData)) as PowerUpSpawnData;
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Create New PowerUp Spawn Data"))
        {
            powerUpSpawnData = CreateInstance<PowerUpSpawnData>();
            powerUpSpawnData.SpawnAreas = new List<SpawnRangeData>();
            AssetDatabase.CreateAsset(powerUpSpawnData, kSpawnAreaDataFileName);
            AssetDatabase.SaveAssets();
        }

        GUILayout.Label("Add Spawn Area");

        newSpawnAreaName = EditorGUILayout.TextField("Area Name : ", newSpawnAreaName);

        if (GUILayout.Button("Add New Spawn Area"))
        {
            if (!string.IsNullOrEmpty(newSpawnAreaName))
            {
                SpawnRangeData spawnRangeData = CreateInstance<SpawnRangeData>();
                spawnRangeData.name = newSpawnAreaName;
                powerUpSpawnData.SpawnAreas.Add(spawnRangeData);
                AssetDatabase.AddObjectToAsset(spawnRangeData, powerUpSpawnData);
                AssetDatabase.SaveAssets();
                newSpawnAreaName = string.Empty;
            }
            else
            {
                EditorUtility.DisplayDialog("Salad Chef", "Enter spawn area name", "OK");
            }
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(powerUpSpawnData);
        }

        Repaint();
    }
}
