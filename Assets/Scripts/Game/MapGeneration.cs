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
  
    private ObjectPoolerManager objectPoolerManager;
    private GameManager gameManager;
    private PlayerData playerData;
    private int currentLevel;
    public float delaySpawnEnemy;
    private int totalEnemyInWave;
    private int wave;
    private int totalWave;
    private UIGameProcess uIGameProcess;

    private void Awake()
    {
        objectPoolerManager = ObjectPoolerManager.Instance;
        gameManager = GameManager.Instance;
    }

    private void OnEnable() 
    {
        gameManager.OnStartGame += StartGame;
        gameManager.OnEnemyDeath += CountEnemyDeath;
        gameManager.OnNewGame += NewGame;

    }

    private void OnDisable() 
    {
        gameManager.OnStartGame -= StartGame;
        gameManager.OnEnemyDeath -= CountEnemyDeath;
        gameManager.OnNewGame -= NewGame;

    }

    private void StartGame()
    {
        playerData = PlayerData.Load();
        currentLevel = playerData.LatestLevel;
        this.wave = 0;
        this.totalWave = levels[currentLevel].wave.Length;
        SetPositionSpawnEnemyInWave(currentLevel,wave);
        uIGameProcess = FindObjectOfType<UIGameProcess>();
        uIGameProcess.CreateProcessBar(levels[currentLevel].process);
    }

    private void NewGame()
    {
        ClearEnemies();
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

    IEnumerator SpawnEnemy(int level, int wave, int enemyIndex, Vector3 enemyPosition) {
        yield return new WaitForSeconds(delaySpawnEnemy);
        objectPoolerManager.SpawnObject(levels[level].wave[wave].GetEnemyGameObjectPool(enemyIndex), enemyPosition, Quaternion.identity);
    }

    private void CountEnemyDeath()
    {
        if(wave != totalWave)
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
        SetPositionSpawnEnemyInWave(currentLevel,this.wave);
    }


    private void ClearEnemies() {
        objectPoolerManager.DeleteObjectPoolerManager();
    }
}
