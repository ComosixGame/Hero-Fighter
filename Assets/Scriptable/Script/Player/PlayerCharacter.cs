using UnityEngine;
using MyCustomAttribute;

[System.Serializable]
public class PlayerCharacter
{
    [Label("KeyID (requrie & unquie)")] public string keyID;
    public Sprite thumbnail;
    public string name;
    public int price;
    public int start;
    public string evaluate;
    public GameObject character;
}
