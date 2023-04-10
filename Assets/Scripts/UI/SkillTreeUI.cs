using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillTreeUI : MonoBehaviour
{ 
    [SerializeField] private List<Button> choiceHeroBtn;
    [SerializeField] private List<TextMeshProUGUI> levelSkill1Text;
    [SerializeField] private List<TextMeshProUGUI> levelSkill2Text;
    [SerializeField] private List<TextMeshProUGUI> levelSkill3Text;
    [SerializeField] private List<TextMeshProUGUI> levelSkill4Text;
    [SerializeField] private List<TextMeshProUGUI> levelSkill1PriceText;
    [SerializeField] private List<TextMeshProUGUI> levelSkill2PriceText;
    [SerializeField] private List<TextMeshProUGUI> levelSkill3PriceText;
    [SerializeField] private List<TextMeshProUGUI> levelSkill4PriceText;

    private GameManager gameManager;
    private PlayerData playerData;
    

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    private void OnEnable()
    {
        playerData = PlayerData.Load();
        LoadSkillLevel(0);
        GetPriceSkill(0);
        for (int i = 0; i < playerData.characters.Count; i++)
        {
            choiceHeroBtn[i].interactable = true;
        }
    }


    public void UpgradeSkill1Hero(int id)
    {
        playerData = PlayerData.Load();
        if (playerData.money > 5)
        {
            levelSkill1Text[id].text = "Level Skill: "+ playerData.characters[id].skill1;
            playerData.money = playerData.money - 5;
            playerData.UpgradeSkill1(id);
            playerData.Save();
        }
        else
        {
            gameManager.BuyFailure();
        }
    }

    public void UpgradeSkill2Hero(int id)
    {
        playerData = PlayerData.Load();
        if (playerData.money > 5)
        {
            levelSkill2Text[id].text = "Level Skill: "+ playerData.characters[id].skill2;
            playerData.money = playerData.money - 5;
            playerData.UpgradeSkill2(id);
            playerData.Save();
        }
        else
        {
            gameManager.BuyFailure();
        }
    }

    public void UpgradeSkill3Hero(int id)
    {
        playerData = PlayerData.Load();
        if (playerData.money > 5)
        {
            levelSkill3Text[id].text = "Level Skill: "+ playerData.characters[id].skill3;
            playerData.money = playerData.money - 5;
            playerData.UpgradeSkill3(id);
            playerData.Save();
        }
        else
        {
            gameManager.BuyFailure();
        }
    }

    public void UpgradeSkill4Hero(int id)
    {
        playerData = PlayerData.Load();
        if (playerData.money > 5)
        {
            levelSkill4Text[id].text = "Level Skill: "+ playerData.characters[id].skill4;
            playerData.money = playerData.money - 5;
            playerData.UpgradeSkill4(id);
            playerData.Save();
        }
        else
        {
            gameManager.BuyFailure();
        }
    }

    public void LoadSkillLevel(int characterId)
    {
        levelSkill1Text[characterId].text = "Level Skill: "+ playerData.characters[characterId].skill1;
        levelSkill2Text[characterId].text = "Level Skill: "+ playerData.characters[characterId].skill2;
        levelSkill3Text[characterId].text = "Level Skill: "+ playerData.characters[characterId].skill3;
        levelSkill4Text[characterId].text = "Level Skill: "+ playerData.characters[characterId].skill4;
    }

    public void GetPriceSkill(int characterId)
    {
        //Load Price from PlayerData
        levelSkill1PriceText[characterId].text = ""+"";
        levelSkill2PriceText[characterId].text = ""+"";
        levelSkill3PriceText[characterId].text = ""+"";
        levelSkill4PriceText[characterId].text = ""+"";

    }

}

