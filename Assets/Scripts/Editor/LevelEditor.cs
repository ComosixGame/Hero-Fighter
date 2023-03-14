using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{
    private GameObjectPool enemy;
    private int enemyIndex;

    public static void ShowWindow()
    {
        var window = GetWindow<LevelEditor>();
        window.titleContent = new GUIContent("Level Editor");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Add new Enemy");
        enemy = EditorGUILayout.ObjectField("Enemy", enemy, typeof(GameObjectPool), false) as GameObjectPool;
        EditorGUILayout.Space();
        if (GUILayout.Button("Add"))
        {
            if (enemy == null)
            {
                EditorUtility.DisplayDialog(
                    "Error",
                    "Enemy cant be none",
                    "OK"
                );
            }
            else
            {
                MapGenerationEditor.AddEnemy(enemy);
            }
        }
        EditorGUILayout.Space(50);
        GUILayout.Label("Delete Enemy");
        enemyIndex = EditorGUILayout.IntField("enemy inedex", enemyIndex);
        EditorGUILayout.Space();
        if (GUILayout.Button("Delete"))
        {
            MapGenerationEditor.RemoveEnemy(enemyIndex);
        }

    }
}