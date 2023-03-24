using UnityEngine;

public class ChapterGeneration : MonoBehaviour
{
    public ChapterManager chapterManager;
    public MapGeneration mapGeneration;
    public int chapterIndex;
    public int levelIndex;
    private int currentChapter;
    private int totalLevelInChapter;
    private PlayerData playerData;
    private GameManager gameManager;

    private void Awake()
    {
        playerData = PlayerData.Load();
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        gameManager.OnStartGame += StartGame;
        gameManager.OnEndGame += EndGame;
    }

    private void OnDisable()
    {
        gameManager.OnStartGame -= StartGame;
        gameManager.OnEndGame += EndGame;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        mapGeneration.levelState = chapterManager.chapterStates[chapterIndex].levelStates[levelIndex];
    }

    public void EndGame(bool isWin)
    {
        playerData = PlayerData.Load();
        if (totalLevelInChapter-1 == playerData.LatestChapter)
        {
            currentChapter +=1;
            playerData.chapters.Add(currentChapter);
            playerData.levels.Clear();
            playerData.Save();
        }
    }


}
