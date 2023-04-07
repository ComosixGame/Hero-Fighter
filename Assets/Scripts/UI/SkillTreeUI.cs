using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillTreeUI : MonoBehaviour
{
    public HeroCardSkillTree heroCardSkillTree;
    public Transform contentHero;
    private GameManager gameManager;
    private PlayerData playerData;


    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start() {
        RenderHeroCard();
    }


    public void RenderHeroCard()
    {
        playerData = PlayerData.Load();
        List<PlayerData.Character> characters = playerData.characters;
        EquipmentManager equipmentManager = gameManager.equipmentManager;
        for (int i = 0; i < playerData.characters.Count; i++)
        {
            HeroCardSkillTree heroCardSkillTreeInit = Instantiate(heroCardSkillTree);
            heroCardSkillTreeInit.GetComponent<HeroCardSkillTree>();
            heroCardSkillTreeInit.SetDataCard(i, equipmentManager.GetCharacter(characters[i].keyID), gameObject.GetComponent<SkillTreeUI>());
            heroCardSkillTreeInit.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            heroCardSkillTreeInit.transform.SetParent(contentHero.transform, false);
        }
    }


}
