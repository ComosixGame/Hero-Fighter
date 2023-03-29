using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterManagerUI : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject contentChapter;
    public GameObject cardChapter;
    public GameObject windowSelectLevel;
    private PlayerData playerData;
    private List<ChapterCard> chapterCards = new List<ChapterCard>();

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {


    }

    private void Start()
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

    public void DisplayChapterUI()
    {
        ChapterState[] chapterStates =  gameManager.chapterManager.chapterStates;

        for (int l = 0; l < chapterStates.Length; l++)
        {
            GameObject chapterInit = Instantiate(cardChapter);
            ChapterCard newCardChapter = chapterInit.GetComponent<ChapterCard>();
            newCardChapter.id = l;
            newCardChapter.windowSelectLevel = windowSelectLevel;
            //Change name Chapter
            newCardChapter.SetDataCard(chapterStates[l].sprite, chapterStates[l].name);
            newCardChapter.transform.SetParent(contentChapter.transform, false);
            chapterCards.Add(newCardChapter);
        }

        playerData = PlayerData.Load();
        int chapter = playerData.LatestChapter;

        for (int i = 0; i < chapterCards.Count; i++)
        {
            chapterCards[i].Unlock(i <= chapter);
        }
    }
}

