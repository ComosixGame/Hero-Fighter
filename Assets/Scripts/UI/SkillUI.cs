using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    public SkillsManager skillsManager;

    public GameObject skillDetail;

    //Change video (Asign Event OnClick In Editor)
    public void GetSkill(int index)
    {
        skillDetail.GetComponent<UnityEngine.Video.VideoPlayer>().clip = skillsManager.skills[index].skill.gameObject.GetComponent<UnityEngine.Video.VideoPlayer>().clip;
    }
}
