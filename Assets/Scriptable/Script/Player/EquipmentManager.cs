using UnityEngine;

[CreateAssetMenu(fileName = "Character Manager", menuName = "Equipment/Equipment Manager")]
public class EquipmentManager : ScriptableObject
{
    public PlayerCharacter[] Characters;

    public int CharactersCount {
        get {
            return Characters.Length;
        }
    }
}