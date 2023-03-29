using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;

public class SkillType
{
    public const string flipJump = "FlipJump";
}

public class SpecialSkillType
{
    public const string fireBall = "FireBall";
}

public class SkillSystem : MonoBehaviour
{
    [ReadOnly] public AbsSpecialSkill specialSkillAvailable;
    [ReadOnly] public List<AbsSkill> skillsAvailable;
    private string selectedCharacter;

    private void Start()
    {
        skillsAvailable =  new List<AbsSkill>();

        AbsSkill[] skills = GetComponents<AbsSkill>();
        AbsSpecialSkill[] specialSkills = GetComponents<AbsSpecialSkill>();
        
        foreach(AbsSkill skill in skills) {
            skill.enabled = false;
        }

        foreach(AbsSpecialSkill specialSkill in specialSkills) {
            specialSkill.enabled = false;
        }

        SetAvailableSkills();

        GetComponent<PlayerController>().AddSkill(skillsAvailable.ToArray(), specialSkillAvailable);
    }

    private void SetAvailableSkills(){

        PlayerData playerData = PlayerData.Load();
        selectedCharacter = playerData.selectedCharacter;
        var character = playerData.characters.Single(character => character.keyID == selectedCharacter);
        foreach(string skillName in character.selectedSkill) {
            AbsSkill skill = GetComponent(Type.GetType(skillName)) as AbsSkill;
            skill.enabled = true;
            skillsAvailable.Add(skill);
        }
        if(!String.IsNullOrEmpty(character.selectedSpecialSkill)) {
            specialSkillAvailable = GetComponent(Type.GetType(character.selectedSpecialSkill)) as AbsSpecialSkill;
            specialSkillAvailable.enabled = true;
        }

    }
}
