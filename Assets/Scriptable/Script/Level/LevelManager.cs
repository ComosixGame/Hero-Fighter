using UnityEngine;

[CreateAssetMenu(fileName = "New LevelManager", menuName = "Scriptable Manager/LevelManager")]
public class LevelManager : ScriptableObject
{
    public LevelState[] levels;
}