using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManagerUI : MonoBehaviour
{
    private GameManager gameManager;
    private int currentHeroId = -1;
    private int currentSkillId = -1;
    private List<GameObject> children;

    //Hero UI
    [Header("HeroUI")]
    [SerializeField] private GameObject contentHero;
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

    // Start is called before the first frame update
    void Start()
    {
        RenderHeroCardUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RenderHeroCardUI()
    {
        PlayerCharacter[] playerCharacters = gameManager.equipmentManager.Characters;

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            GameObject heroCardInit = Instantiate(cardHero);
            HeroCardSkill newCardHeroSkill = heroCardInit.GetComponent<HeroCardSkill>();
            newCardHeroSkill.UIMenu = UIMenu;
            newCardHeroSkill.skillTreeUI = skillTreeUI;
            newCardHeroSkill.windowSkill = windowSkill;
            newCardHeroSkill.id = i;
            newCardHeroSkill.SetDataCard(playerCharacters[i].thumbnail, playerCharacters[i].name, playerCharacters[i].start);
            newCardHeroSkill.transform.SetParent(contentHero.transform, false);
        }
    }
}
