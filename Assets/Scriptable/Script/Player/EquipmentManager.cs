using System;
using System.Collections.Generic;
using System.Linq;
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
        return Characters.Single(character => character.keyID == id);
    }

    private void OnValidate() {
        defaultCharacterStatic = defaultCharacter;
    }
}