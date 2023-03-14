
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(MapGeneration))]
public class MapGenerationEditor : Editor
{
    private static LevelState levelState;
    private static int waveIndex;

    private void OnEnable()
    {
        MapGeneration mapGeneration = target as MapGeneration;
        levelState = mapGeneration.levelState;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space(20);
        GUILayout.Label($"Max Index Wave: {levelState.waves.Count - 1}");
        EditorGUI.BeginChangeCheck();
        int wave = EditorGUILayout.IntField("Wave Editting", waveIndex);
        if (EditorGUI.EndChangeCheck())
        {
            waveIndex = wave < levelState.waves.Count ? wave : levelState.waves.Count - 1;
        }
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New Wave"))
        {
            levelState.waves.Add(new LevelState.Wave());
            waveIndex = levelState.waves.Count - 1;
        }
        if (GUILayout.Button($"Remove Wave At Index {waveIndex}"))
        {
            if (waveIndex >= 0 && waveIndex < levelState.waves.Count)
            {
                levelState.waves.RemoveAt(waveIndex);
                waveIndex = 0;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(20);

        if (GUILayout.Button("More Option"))
        {
            LevelEditor.ShowWindow();
        }
    }

    private void OnSceneGUI()
    {
        if (waveIndex >= 0 && waveIndex < levelState.waves.Count)
        {
            LevelState.Wave wave = levelState.waves[waveIndex];
            foreach (var (enemy, i) in wave.enemies.Select((val, i) => (val, i)))
            {
                GameObject prefab = enemy.enemyObjectPool.GetGameObject();
                Handles.Label(enemy.position, prefab.name + ' ' + i, "TextField");
                EditorGUI.BeginChangeCheck();
                Vector3 pos = Handles.PositionHandle(enemy.position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    UnityEditor.Undo.RecordObject(levelState, "Update enemy position");
                    enemy.position = pos;
                    EditorUtility.SetDirty(levelState);
                }
                Quaternion rot = Handles.RotationHandle(Quaternion.Euler(enemy.eulerRotation), enemy.position);
                if (EditorGUI.EndChangeCheck())
                {
                    UnityEditor.Undo.RecordObject(levelState, "Update enemy rotation");
                    enemy.eulerRotation = rot.eulerAngles;
                    EditorUtility.SetDirty(levelState);
                }
            }
        }
    }

    public static void AddEnemy(GameObjectPool enemy)
    {
        levelState.waves[waveIndex].enemies.Add(new LevelState.Enemy(enemy));
    }
    public static void RemoveEnemy(int index)
    {
        if (index >= 0 && index < levelState.waves[waveIndex].enemies.Count)
        {
            levelState.waves[waveIndex].enemies.RemoveAt(index);
        }
    }
}