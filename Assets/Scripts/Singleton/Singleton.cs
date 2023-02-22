using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Component
{
    private static T _instance;
    public static T Instance {
        get {
            if(_instance == null) {
               _instance = GameObject.FindObjectOfType<T>();
               if(_instance == null) {
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
               }
            }
            return _instance;
        }
    }

    protected virtual void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }

}
