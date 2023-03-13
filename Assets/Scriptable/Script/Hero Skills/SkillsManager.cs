using UnityEngine;

[CreateAssetMenu(fileName = "Skills Manager", menuName = "Skills/Skills Manager")]
public class SkillsManager : ScriptableObject
{
    public HeroSkills[] skills;

    public int SkillCount {
        get {
            return skills.Length;
        }
    }
}
