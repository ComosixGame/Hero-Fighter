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
    [SerializeField] private Transform commingSoonCard;
    [SerializeField] private ScrollRect scrollRect;
    private RectTransform currentCard;
    private List<ChapterCard> chapterCards = new List<ChapterCard>();

    private void Awake()
    {
        gameManager = GameManager.Instance;
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
        int latestChapter = PlayerData.Load().latestChapter;

        for (int i = 0; i < chapterStates.Length; i++)
        {
            GameObject chapterInit = Instantiate(cardChapter);
            ChapterCard newCardChapter = chapterInit.GetComponent<ChapterCard>();
            newCardChapter.id = i;
            newCardChapter.windowSelectLevel = windowSelectLevel;
            //Change name Chapter
            newCardChapter.SetDataCard(chapterStates[i].sprite, chapterStates[i].name);
            newCardChapter.transform.SetParent(contentChapter.transform, false);
            chapterCards.Add(newCardChapter);
            chapterCards[i].Unlock(i <= latestChapter);
            if(i == latestChapter) {
                ScrollTo(chapterInit.GetComponent<RectTransform>());
            }
        }

        commingSoonCard.SetAsLastSibling();
    }

    public void ScrollTo(RectTransform target)
    {
        Ultils.ScrollTo(scrollRect, target);
        currentCard = target;
    }
}

