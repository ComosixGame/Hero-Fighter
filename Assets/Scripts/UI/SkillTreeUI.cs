using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] private Transform contentHero;
    [SerializeField] private Button heroBtn;
    [SerializeField] private TextMeshProUGUI heroTitle;
    [SerializeField] private Image heroSprite;
    [SerializeField] private Sprite selectedBoder;
    //Skill
    [SerializeField] private Transform contentSkill;
    [SerializeField] private SkillDetailCard skillDetailCard;

    private GameManager gameManager;


    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        RenderHeroButton();
    }


    private void RenderHeroButton()
    {
        PlayerData playerData = PlayerData.Load();
        List<PlayerData.Character> characters = playerData.characters;
        string selectedCharacter = playerData.selectedCharacter;
        RenderSkillDetail(selectedCharacter);
        EquipmentManager equipmentManager = gameManager.equipmentManager;
        
        for (int i = 0; i < characters.Count; i++)
        {
            Button heroBtnInit = Instantiate(heroBtn);
            bool selected = characters[i].keyID == selectedCharacter;
            SetDataButton(heroBtnInit, selected, i);
            heroTitle.text = equipmentManager.Characters[i].name;
            heroSprite.sprite = equipmentManager.Characters[i].thumbnail;
            heroBtnInit.GetComponentInChildren<TextMeshProUGUI>().text = equipmentManager.Characters[i].name;
            heroBtnInit.transform.SetParent(contentHero.transform, false);
        }
    }

    private void RenderSkillDetail(string charaterId)
    {
        PlayerData playerData = PlayerData.Load();
        EquipmentManager equipmentManager = gameManager.equipmentManager;
        PlayerCharacter playerCharacter = equipmentManager.GetCharacter(charaterId);
        for (int i = 0; i < playerCharacter.skillStates.Length; i++)
        {
            SkillDetailCard skillDetailCardInit = Instantiate(skillDetailCard);
            skillDetailCardInit.id = i;
            int index =  playerData.characters.FindIndex(x => x.keyID == charaterId);
            string skillLevel = "Level Skill: " + playerData.characters[index].levelSkills[i];
            skillDetailCardInit.SetDataCard(playerCharacter, i, skillLevel);
            skillDetailCardInit.transform.SetParent(contentSkill.transform, false);
        }

    }

    private void SetDataButton(Button button ,bool selected, int id)
    {
        if (selected)
        {
            button.interactable = false;
            button.GetComponent<Image>().sprite = selectedBoder;
        }else
        {
            button.interactable = true;
        }
        // button.onClick.
    }

}

