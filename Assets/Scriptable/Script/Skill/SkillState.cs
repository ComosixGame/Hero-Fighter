using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillState", menuName = "Scriptable/SkillState")]
public class SkillState : ScriptableObject
{
    public string nameSkill;
    public int price;
    public Sprite sprite;

    //Dont Use Element 0, Used Element 1 
    public int[] attritionEnergys;
    //Dont Use Element 0, Used Element 1 
    public int[] maxCoolDownTimes;
    //Dont Use Element 0, Used Element 1 
    public int[] damages;
}
