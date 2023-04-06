using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManagerUI : MonoBehaviour
{
    private GameManager gameManager;
    private int currentHeroId = -1;
    private int currentSkillId = -1;
    private List<GameObject> children;

    //Hero UI
    [Header("HeroUI")]
    [SerializeField] private Transform contentHero;
    [SerializeField] private GameObject cardHero;
    [SerializeField] private ScrollRect scrollRect;
    public SkillTreeUI skillTreeUI;
    [SerializeField] private GameObject windowSkill;
    [SerializeField] private Animator UIMenu;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        children = new List<GameObject>();
    }

    void OnEnable()
    {
        RenderHeroCardUI();
    }

    private void RenderHeroCardUI()
    {
        ClearChidlren();
        PlayerData playerData = PlayerData.Load();
        List<PlayerData.Character> characters = playerData.characters;
        string selectedCharacter = playerData.selectedCharacter;
        EquipmentManager equipmentManager = gameManager.equipmentManager;

        for (int i = 0; i < characters.Count; i++)
        {
            GameObject heroCardInit = Instantiate(cardHero);
            HeroCardSkill newCardHeroSkill = heroCardInit.GetComponent<HeroCardSkill>();
            newCardHeroSkill.UIMenu = UIMenu;
            newCardHeroSkill.skillTreeUI = skillTreeUI;
            newCardHeroSkill.windowSkill = windowSkill;
            newCardHeroSkill.id = i;
            bool selected = characters[i].keyID == selectedCharacter;
            newCardHeroSkill.SetDataCard(equipmentManager.GetCharacter(characters[i].keyID), selected);
            newCardHeroSkill.transform.SetParent(contentHero, false);
        }
    }

    private void ClearChidlren() {
        for(int i = 0; i < contentHero.childCount; i++) {
            Destroy(contentHero.GetChild(i).gameObject);
        }
    }
}
