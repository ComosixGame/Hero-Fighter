using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterManagerUI : MonoBehaviour
{
    public Chapter[] chapter;
    public GameObject currentLevelImage;
    public GameObject doneLevelImage;
    public GameObject levelExample;
    private ChapterGeneration chapterGeneration;
    private PlayerData playerData;
    private int levelIndex;
    private int chapterIndex;
    private GameManager gameManager;
    public GameObject contentChapter;
    public GameObject cardChapter;
    public GameObject windowSelectLevel;

    private void Awake() 
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable() 
    {
        DisplayLevelUI();
    }

    private void Start() {
        DisplayChapterUI();
    }


    public void SelectChapter(int chapter)
    {
       gameManager.chapterIndex = chapter;
    }

    public void SelectLevel(int level)
    {
        gameManager.levelIndex = level;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayLevelUI()
    {
        playerData = PlayerData.Load();
        levelIndex = playerData.LatestLevel;
        chapterIndex = playerData.LatestChapter;
        if (levelIndex != 0)
        {
            //Level Did play
            for (int j= 0; j < levelIndex; j++)
            {
                chapter[chapterIndex].GetButton(j).GetComponent<Button>().targetGraphic.GetComponent<Image>().gameObject.SetActive(false);
                chapter[chapterIndex].GetButton(j).GetComponent<Image>().sprite = doneLevelImage.GetComponent<Image>().sprite;
                chapter[chapterIndex].GetButton(j).GetComponent<Button>().targetGraphic= chapter[chapterIndex].GetButton(j).GetComponent<Image>();
            }
            
            //Level new
            for (int i = levelIndex; i < chapter[chapterIndex].levelBtns.Count; i++)
            {
                chapter[chapterIndex].GetButton(i).GetComponent<Button>().targetGraphic= levelExample.GetComponent<Image>();
                chapter[chapterIndex].GetButton(i).GetComponent<Button>().interactable = false;
                chapter[chapterIndex].GetText(i).gameObject.SetActive(false);
            }

            //Level current
            chapter[chapterIndex].GetButton(levelIndex-1).GetComponent<Image>().sprite = currentLevelImage.GetComponent<Image>().sprite;
        }
        else
        {
            for (int k = 1; k < chapter[chapterIndex].levelBtns.Count; k++)
            {
                chapter[chapterIndex].GetButton(k).GetComponent<Button>().targetGraphic= levelExample.GetComponent<Image>();
                chapter[chapterIndex].GetButton(k).GetComponent<Button>().interactable = false;
                chapter[chapterIndex].GetText(k).gameObject.SetActive(false);
            }

            chapter[chapterIndex].GetButton(0).GetComponent<Button>().targetGraphic.GetComponent<Image>().gameObject.SetActive(false);
            chapter[chapterIndex].GetButton(0).GetComponent<Image>().sprite = currentLevelImage.GetComponent<Image>().sprite;
        }
    }

    public void DisplayChapterUI()
    {
        for (int l = 0; l < gameManager.chapterManager.chapterStates.Length; l++)
        {
            GameObject chapterInit = Instantiate(cardChapter);
            ChapterCard chapterCard = chapterInit.GetComponent<ChapterCard>();
            chapterCard.id = l;
            chapterCard.windowSelectLevel = windowSelectLevel;
            //Change name Chapter
            chapterInit.GetComponentInChildren<TMP_Text>().text = gameManager.chapterManager.chapterStates[l].name;
            //Change sprite map
            chapterInit.GetComponent<Image>().sprite = gameManager.chapterManager.chapterStates[l].sprite;
            chapterInit.transform.SetParent(contentChapter.transform, false);
        }
    }

}

[Serializable]
public class Chapter
{
    public string name;
    public List<Button> levelBtns;
    public List<GameObject> levelTxt;
    public Button GetButton(int id)
    {
        return levelBtns[id];
    }

    public GameObject GetText(int id)
    {
        return levelTxt[id];
    }
}
