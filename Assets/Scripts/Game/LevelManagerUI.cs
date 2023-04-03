using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManagerUI : MonoBehaviour
{
    public Transform contentLevel;
    public LevelCard levelCard, currentLevelCard, bossCard;
    [SerializeField] private GameObject levelGroup;
    [SerializeField] private ChapterManager chapterManager;
    [SerializeField] private TextMeshProUGUI titleChapter;
    [SerializeField] private ScrollRect scrollRect;
    private GameManager gameManager;
    private int currentChapter = -1;
    private List<GameObject> children;
    private List<LevelCard> levelCards = new List<LevelCard>();
    private RectTransform currentCard;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        children = new List<GameObject>();
    }

    private void OnEnable()
    {
        if (currentCard)
        {
            ScrollTo(currentCard);
        }
    }

    private void Update()
    {
        if (currentChapter != gameManager.chapterIndex)
        {
            currentChapter = gameManager.chapterIndex;
            RenderLevelCard(currentChapter);
        }
    }

    private void RenderLevelCard(int chapter)
    {
        foreach (GameObject child in children)
        {
            Destroy(child);
        }

        int step = 0;
        Vector2 anchor;
        GameObject newLevelGroup = levelGroup;
        LevelState[] levelStates = chapterManager.chapterStates[chapter].levelStates;
        titleChapter.text = chapterManager.chapterStates[chapter].name;
        int latestLevel = PlayerData.Load().latestLevel;
        for (int i = 0; i < levelStates.Length; i++)
        {
            LevelCard lv;
            if (levelStates[i].type == LevelType.normal)
            {
                step++;
                if (step >= 4)
                    step = 0;
                switch (step)
                {
                    case 1:
                        newLevelGroup = Instantiate(levelGroup);
                        newLevelGroup.transform.SetParent(contentLevel, false);
                        newLevelGroup.SetActive(true);

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

                if (i == latestLevel)
                {
                    lv = Instantiate(currentLevelCard, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    lv = Instantiate(levelCard, Vector3.zero, Quaternion.identity);
                }

                lv.chapter = currentChapter + 1;
                int index = i + 1;
                lv.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
                lv.id = i;
                lv.Unlock(i <= latestLevel);
                RectTransform rectTransform = lv.GetComponent<RectTransform>();
                rectTransform.anchorMax = anchor;
                rectTransform.anchorMin = anchor;
                lv.transform.SetParent(newLevelGroup.transform, false);

                children.Add(newLevelGroup);
                levelCards.Add(lv);
            }
            else
            {
                lv = Instantiate(bossCard);
                lv.chapter = currentChapter + 1;
                lv.GetComponentInChildren<TextMeshProUGUI>().text = "Boss";
                lv.id = i;
                lv.Unlock(i <= latestLevel);
                lv.transform.SetParent(contentLevel, false);
                children.Add(lv.gameObject);
                levelCards.Add(lv);
            }

            if (i == latestLevel)
            {
                ScrollTo(lv.GetComponent<RectTransform>());
            }
        }
    }

    public void ScrollTo(RectTransform target)
    {
        Ultils.ScrollTo(scrollRect, target);
        currentCard = target;
    }
}
