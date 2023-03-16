using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChapterManager", menuName = "Scriptable/ChapterManager")]
public class ChapterManager : ScriptableObject
{
    public ChapterState[] chapterStates;
}

[Serializable]
public class ChapterState
{
    public string name;
    public LevelState[] levelStates;

}
