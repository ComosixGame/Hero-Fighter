using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManagerUI : MonoBehaviour
{
    //Skill UI
    [Header("SkillUI")]
    public SkillCard skillCard;
    public Transform contentSkill;
    [SerializeField] private GameObject skillGroup;
    [SerializeField] private TextMeshProUGUI titleSkill;
    [SerializeField] private SkillTreeManager skillTreeManager;
    [SerializeField] private GameObject windowSkillDetail;
    [SerializeField] private GameObject windowSkill;
    [SerializeField] private Animator UIMenu;

    private GameManager gameManager;
    private int currentHeroId = -1;
    private int currentSkillId = -1;
    private List<GameObject> children;

    //Hero UI
    [Header("HeroUI")]
    [SerializeField] private GameObject contentHero;
    [SerializeField] private GameObject cardHero;
    [SerializeField] private ScrollRect scrollRect;


    private void Awake()
    {
        gameManager = GameManager.Instance;
        children = new List<GameObject>();
    }

    private void OnEnable() {
        
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

    private void ClickButton()
    {
        RenderSkillCard(0);
    }

    private void RenderHeroCardUI()
    {
        PlayerCharacter[] playerCharacters = gameManager.equipmentManager.Characters;

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            GameObject heroCardInit = Instantiate(cardHero);
            HeroCard newCardHero = heroCardInit.GetComponent<HeroCard>();
            newCardHero.windowSkill = windowSkill;
            newCardHero.id = i;
            newCardHero.SetDataCard(playerCharacters[i].thumbnail, playerCharacters[i].name, playerCharacters[i].start);
            newCardHero.transform.SetParent(contentHero.transform, false);
        }
    }

    private void RenderSkillCard(int heroId)
    {
        foreach (GameObject child in children)
        {
            Destroy(child);
        }

        int step = 0;
        Vector2 anchor;
        GameObject newSkillGroup = skillGroup;
        SkillState[] skillStates = skillTreeManager.skillTreeStates[heroId].skillStates;
        titleSkill.text = skillTreeManager.skillTreeStates[heroId].name;
        for (int i = 0; i < skillStates.Length; i++)
        {
            SkillCard sk;
            step++;
            if (step >= 4)
                step = 0;
            switch (step)
            {
                case 1:
                    newSkillGroup = Instantiate(skillGroup);
                    newSkillGroup.transform.SetParent(contentSkill, false);
                    newSkillGroup.SetActive(true);

                    anchor = new Vector2(0, 0.5f);
                    break;
                case 2:
                    anchor = new Vector2(0.5f, 1);
                    break;
                case 3:
                    anchor = new Vector2(0.5f, 0);
                    break;
                default:
                    anchor = new Vector2(1, 0.5f);
                    break;
            }
            sk = Instantiate(skillCard, Vector3.zero, Quaternion.identity);
            RectTransform rectTransform = sk.GetComponent<RectTransform>();
            sk.windowSkillTreeDetail = windowSkillDetail;
            sk.currentId = currentSkillId;
            sk.sprite.GetComponent<Image>().sprite = skillStates[i].sprite;
            sk.UIMenu = UIMenu;
            sk.skillId = i;
            rectTransform.anchorMax = anchor;
            rectTransform.anchorMin = anchor;
            sk.transform.SetParent(newSkillGroup.transform, false);
        }


    }
}
