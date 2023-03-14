using UnityEngine;

[CreateAssetMenu(fileName = "New ObjectPoolerManager", menuName = "Scriptable Manager/ObjectPoolerManager")]
public class ObjectPoolerScriptable : ScriptableObject
{
    [System.Serializable]
    public class ScripblePrefab {
        public string name;
        public int size;    
        public GameObjectPool GameObjectPool;
    }
    public ScripblePrefab[] prefabs;
}
