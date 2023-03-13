using UnityEngine;

[CreateAssetMenu(fileName = "New ObjectPoolerManager Manager", menuName = "Scriptable Manager/ObjectPoolerManager Manager")]
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
