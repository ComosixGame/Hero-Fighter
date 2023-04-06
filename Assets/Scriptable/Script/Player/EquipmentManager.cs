using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Manager", menuName = "Equipment/Equipment Manager")]
public class EquipmentManager : ScriptableObject
{
    public string defaultCharacter;
    public static string defaultCharacterStatic;
    public PlayerCharacter[] Characters;
    public int CharactersCount {
        get {
            return Characters.Length;
        }
    }

    public PlayerCharacter GetCharacter(string id) {
        return Array.Find(Characters, el => el.keyID == id);
    }

    private void OnValidate() {
        defaultCharacterStatic = defaultCharacter;
    }
}