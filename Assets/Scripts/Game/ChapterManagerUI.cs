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

    private PlayerData playerData;
    private int levelIndex;
    private int chapterIndex;

    private void Awake() 
    {
        
    }

    private void OnEnable() 
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayChapterUI();
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

        for (int j= 0; j < levelIndex; j++)
        {
            chapter[chapterIndex].GetButton(j).GetComponent<Button>().targetGraphic.GetComponent<Image>().gameObject.SetActive(false);
            chapter[chapterIndex].GetButton(j).GetComponent<Image>().sprite = doneLevelImage.GetComponent<Image>().sprite;
            chapter[chapterIndex].GetButton(j).GetComponent<Button>().targetGraphic= chapter[chapterIndex].GetButton(j).GetComponent<Image>();
        }
        
        for (int i = levelIndex; i < chapter[chapterIndex].levelBtns.Count; i++)
        {
            chapter[chapterIndex].GetButton(i).GetComponent<Button>().targetGraphic= levelExample.GetComponent<Image>();
            chapter[chapterIndex].GetButton(i).GetComponent<Button>().interactable = false;
        }

        chapter[chapterIndex].GetButton(levelIndex-1).GetComponent<Image>().sprite = currentLevelImage.GetComponent<Image>().sprite;
    }
}

[Serializable]
public class Chapter
{
    public string name;
    public List<Button> levelBtns;
    public Button GetButton(int id)
    {
        return levelBtns[id];
    }
}
