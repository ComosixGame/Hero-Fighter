using TMPro;
using UnityEngine;
using MyCustomAttribute;

[CreateAssetMenu(fileName = "New SkillState", menuName = "Scriptable/SkillState")]
public class SkillState : ScriptableObject
{
    public string nameSkill;
    public int price;
    public Sprite sprite;
    public int level;

}
