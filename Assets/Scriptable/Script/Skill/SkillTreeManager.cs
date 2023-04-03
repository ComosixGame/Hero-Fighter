using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillTreeManager", menuName = "Scriptable/SkillTreeManager")]
public class SkillTreeManager : ScriptableObject
{
   public SkillTreeState[] skillTreeStates;
}

[Serializable]
public class SkillTreeState
{
    public string name;
    public SkillState[] skillStates;
}
