using UnityEngine;
using TMPro;

public class SkillUI : MonoBehaviour
{
    private PlayerData playerData;
    private CharacterSelection characterSelection;
    public SkillsManager skillsManager;
    public GameObject skillDetail;
    public TMP_Text   heroTxt;

    private void OnEnable() 
    {
        playerData = PlayerData.Load();
        characterSelection = FindObjectOfType<CharacterSelection>();
        heroTxt.text = characterSelection.equipmentManager.GetCharacter(playerData.selectedCharacter).name;
    }

    //Change video (Asign Event OnClick In Editor)
    public void GetSkill(int index)
    {
        //Show Video In Card Skill
        skillDetail.GetComponent<UnityEngine.Video.VideoPlayer>().clip = skillsManager.skills[index].skill.gameObject.GetComponent<UnityEngine.Video.VideoPlayer>().clip;
    }
}
