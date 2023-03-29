using UnityEngine;
using MyCustomAttribute;

[System.Serializable]
public class PlayerCharacter
{
    [Label("KeyID (requrie & unquie)")] public string keyID;
    public string name;
    public int price;
    public GameObject character;
}
