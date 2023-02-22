using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public string key;
    public GameObject GetGameObject() {
        return this.gameObject;
    }

}