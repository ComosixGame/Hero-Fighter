// #define DEBUG

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;


public enum SkillType
{
    
}

public enum SpecialSkillType
{
    FireBall,
    Eyeslaser
}

public class SkillSystem : MonoBehaviour
{
    [ReadOnly] public AbsSpecialSkill specialSkillAvailable;
    [ReadOnly] public List<AbsSkill> skillsAvailable;
#if DEBUG
    [SerializeField] private SkillType[] skillTypes;
    [SerializeField] private SpecialSkillType specialSkillType;
#endif
    private string selectedCharacter;

    private void Start()
    {
#if DEBUG
        DebugMode();
#else
        skillsAvailable = new List<AbsSkill>();

        AbsSkill[] skills = GetComponents<AbsSkill>();
        AbsSpecialSkill[] specialSkills = GetComponents<AbsSpecialSkill>();

        foreach (AbsSkill skill in skills)
        {
            skill.enabled = false;
        }

        foreach (AbsSpecialSkill specialSkill in specialSkills)
        {
            specialSkill.enabled = false;
        }

        SetAvailableSkills();

        GetComponent<PlayerController>().AddSkill(skillsAvailable.ToArray(), specialSkillAvailable);
#endif
    }

    private void SetAvailableSkills()
    {

        PlayerData playerData = PlayerData.Load();
        selectedCharacter = playerData.selectedCharacter;
        var character = playerData.characters.Single(character => character.keyID == selectedCharacter);
        // foreach (string skillName in character.selectedSkill)
        // {
        //     AbsSkill skill = GetComponent(Type.GetType(skillName)) as AbsSkill;
        //     skill.enabled = true;
        //     skillsAvailable.Add(skill);
        // }
        // if (!String.IsNullOrEmpty(character.selectedSpecialSkill))
        // {
        //     specialSkillAvailable = GetComponent(Type.GetType(character.selectedSpecialSkill)) as AbsSpecialSkill;
        //     specialSkillAvailable.enabled = true;
        //     specialSkillAvailable.Active();
        // }

    }

    private void DebugMode()
    {
        skillsAvailable = new List<AbsSkill>();

        AbsSkill[] skills = GetComponents<AbsSkill>();
        AbsSpecialSkill[] specialSkills = GetComponents<AbsSpecialSkill>();

        foreach (AbsSkill skill in skills)
        {
            skill.enabled = false;
        }

        foreach (AbsSpecialSkill specialSkill in specialSkills)
        {
            specialSkill.enabled = false;
        }
        foreach (SkillType skillName in skillTypes)
        {
            AbsSkill skill = GetComponent(Type.GetType(skillName.ToString())) as AbsSkill;
            skill.enabled = true;
            skillsAvailable.Add(skill);
        }

        specialSkillAvailable = GetComponent(Type.GetType(specialSkillType.ToString())) as AbsSpecialSkill;
        specialSkillAvailable.enabled = true;
        specialSkillAvailable.Active();

        GetComponent<PlayerController>().AddSkill(skillsAvailable.ToArray(), specialSkillAvailable);
    }
}
