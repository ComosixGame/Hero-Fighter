using UnityEngine;

[CreateAssetMenu(fileName = "New SkillState", menuName = "Scriptable/SkillState")]
public class SkillState : ScriptableObject
{
    public string nameSkill;
    public float price;
    public Sprite sprite;
}
