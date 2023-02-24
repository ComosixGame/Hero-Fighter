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
    public float delaySpawnEnemy;

    private void Awake()
    {
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {   
        SetPositionSpawnEnemyInWave(1,1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetPositionSpawnEnemyInWave(int level, int wave)
    {
        //clear danh sách enemy
        // ClearEnemies();

        int totalEnemyInWave = levels[level].wave[wave].enemiesGameObjectPool.Length;
        for (int i = 0; i < totalEnemyInWave; i++)
        {
            StartCoroutine(SpawnEnemy(level, wave, i, levels[level].wave[wave].enemiesPosition[i]));
        }

        
    }

    IEnumerator SpawnEnemy(int level, int wave, int enemyIndex, Vector3 enemyPosition) {
        yield return new WaitForSeconds(delaySpawnEnemy);
        objectPoolerManager.SpawnObject(levels[level].wave[wave].GetEnemyGameObjectPool(enemyIndex), enemyPosition, Quaternion.identity);
    }

    // private void ClearEnemies() {
    //     gameManager.ClearEnemies();
    // }
}
