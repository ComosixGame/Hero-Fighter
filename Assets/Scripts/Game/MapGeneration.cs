using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;
using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{
    [System.Serializable]
    public class ObjectSpawn
    {
        public string key;
        public Wave[] wave;
        public float process;
        public GameObjectPool wallGameObjectPool;
        public Vector3[] wallPosition; 
        public GameObjectPool GetWallGameObjectPool()
        {
            return (wallGameObjectPool);
        }

        public GameObjectPool checkPointGameObjectPool;
        public Vector3[] checkPointPosition; 
        public GameObjectPool GetcheckPointGameObjectPool()
        {
            return (checkPointGameObjectPool);
        }
    }

    [System.Serializable]
    public class Wave
    {
        public GameObjectPool[] enemiesGameObjectPool;
        public Vector3[] enemiesPosition;
        public GameObjectPool GetEnemyGameObjectPool(int index)
        {
            return (enemiesGameObjectPool[index]);
        }

    }

    public ObjectSpawn[] levels;
    public bool isCheckoint;
    private ObjectPoolerManager objectPoolerManager;
    private List<GameObjectPool> enemiesList = new List<GameObjectPool>();
    private List<GameObjectPool> wallList = new List<GameObjectPool>();

    private GameManager gameManager;
    private PlayerData playerData;
    private int currentLevel;
    public float delaySpawnEnemy;
    private int totalEnemyInWave;
    private int wave;
    private int totalWave;
    private UIGameProcess uIGameProcess;
    private UIMenu ui;

    private void Awake()
    {
        objectPoolerManager = ObjectPoolerManager.Instance;
        gameManager = GameManager.Instance;
        ui = FindObjectOfType<UIMenu>();
    }

    private void OnEnable()
    {
        gameManager.OnStartGame += StartGame;
        gameManager.OnEnemyDeath += CountEnemyDeath;
        gameManager.OnNewGame += NewGame;
        gameManager.OnGoneCheckPoint += GoneCheckPoint;

    }

    private void OnDisable()
    {
        gameManager.OnStartGame -= StartGame;
        gameManager.OnEnemyDeath -= CountEnemyDeath;
        gameManager.OnNewGame -= NewGame;
        gameManager.OnGoneCheckPoint -= GoneCheckPoint;

    }

    private void StartGame()
    {
        enemiesList.Clear();
        wallList.Clear();
        playerData = PlayerData.Load();
        currentLevel = playerData.LatestLevel;
        this.wave = 0;
        this.totalWave = levels[currentLevel].wave.Length;
        InitCheckPoint(currentLevel);
        InitWall(currentLevel, wave);
        SetPositionSpawnEnemyInWave(currentLevel, wave);
        uIGameProcess = FindObjectOfType<UIGameProcess>();
        uIGameProcess.CreateProcessBar(levels[currentLevel].process);
    }

    private void NewGame()
    {
        ClearEnemyList();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetPositionSpawnEnemyInWave(int level, int wave)
    {
        int totalEnemyInWave = levels[level].wave[wave].enemiesGameObjectPool.Length;
        this.totalEnemyInWave = totalEnemyInWave;
        for (int i = 0; i < totalEnemyInWave; i++)
        {
            StartCoroutine(SpawnEnemy(level, wave, i, levels[level].wave[wave].enemiesPosition[i]));
        }
        this.wave++;
    }

    private void InitWall(int level, int wave)
    {
        GameObjectPool go = objectPoolerManager.SpawnObject(levels[level].GetWallGameObjectPool(), levels[level].wallPosition[wave], Quaternion.identity);
        wallList.Add(go);
    }

    private void InitCheckPoint(int level)
    {
        foreach (Vector3 pos in levels[level].checkPointPosition)
        {
            GameObjectPool go = objectPoolerManager.SpawnObject(levels[level].GetcheckPointGameObjectPool(), pos, Quaternion.identity);
        }
    }

    IEnumerator SpawnEnemy(int level, int wave, int enemyIndex, Vector3 enemyPosition)
    {
        yield return new WaitForSeconds(delaySpawnEnemy);
        GameObjectPool go = objectPoolerManager.SpawnObject(levels[level].wave[wave].GetEnemyGameObjectPool(enemyIndex), enemyPosition, Quaternion.identity);
        enemiesList.Add(go);
    }
    private void WallDestroy(int wave)
    {
        Destroy(wallList[wave].gameObject);
    }

    private void CountEnemyDeath()
    {
        if (wave != totalWave)
        {
            if (totalEnemyInWave != 0)
            {
                totalEnemyInWave--;
            }

            if (totalEnemyInWave == 0)
            {
                NewWave();
            }
        }
        else
        {
            gameManager.GameWin();
        }
    }

    private void NewWave()
    {
        ui.PreviousAnimation(true);
        WallDestroy(this.wave-1);
            
    }

    private void GoneCheckPoint()
    {
        ui.PreviousAnimation(false);
        InitWall(currentLevel, this.wave);
        SetPositionSpawnEnemyInWave(currentLevel, this.wave);
        isCheckoint = false;
    }

    private void ClearEnemyList()
    {
        for (int k = 0; k < enemiesList.Count; k++)
        {
            Destroy(enemiesList[k].gameObject);
        }
    }
}
