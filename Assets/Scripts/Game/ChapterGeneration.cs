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
        chapterIndex = gameManager.chapterIndex;
        levelIndex = gameManager.levelIndex;
    }

    private void OnDisable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
