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
    [SerializeField] private Sprite normalBoder;
    private int index = 0;

    //Skill
    [SerializeField] private Transform contentSkill;
    [SerializeField] private SkillDetailCard skillDetailCard;
    private List<GameObject> children;

    private GameManager gameManager;


    private void Awake()
    {
        gameManager = GameManager.Instance;
        children = new List<GameObject>();
    }

    private void OnEnable() {
        gameManager.OnBuyHero += UnLockNewButtonHero;
    }

    private void OnDisable() {
        gameManager.OnBuyHero -= UnLockNewButtonHero;
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
            if (selected) index = i;
            heroBtnInit.GetComponent<CardChoiceHeroSkillUI>().SetDataButton(selected, selectedBoder, normalBoder, equipmentManager.GetCharacter(characters[i].keyID), this);
            heroBtnInit.GetComponentInChildren<TextMeshProUGUI>().text = equipmentManager.Characters[i].name;
            heroBtnInit.transform.SetParent(contentHero.transform, false);
        }

        heroTitle.text = equipmentManager.Characters[index].name;
        heroSprite.sprite = equipmentManager.Characters[index].thumbnail;
    }

    public void RenderSkillDetail(string charaterId)
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
            children.Add(skillDetailCardInit.gameObject);
        }
    }

    public void ChangeInforCard(Sprite thumbnail, string heroName)
    {
        heroSprite.sprite = thumbnail;
        heroTitle.text = heroName;
    }

    
    private void UnLockNewButtonHero()
    {
        RenderHeroButton();
    }

    public void ClearSkillCardData()
    {
        foreach(GameObject child in children)
        {
            Destroy(child);
        }
    }

    

}

