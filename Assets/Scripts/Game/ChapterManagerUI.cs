using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterManagerUI : MonoBehaviour
{
    public Chapter[] chapter;
    public GameObject currentLevelImage;
    public GameObject doneLevelImage;
    public GameObject levelExample;
    [SerializeField] private ChapterManager chapterManager;
    private ChapterGeneration chapterGeneration;
    private PlayerData playerData;
    private int levelIndex;
    private int chapterIndex;
    private GameManager gameManager;

    private void Awake() 
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable() 
    {
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

    public void DisplayChapterUI()
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
            Debug.Log(123);
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
