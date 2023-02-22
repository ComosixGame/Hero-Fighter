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
        public int level;
        //Enemy
        [SerializeField] private GameObjectPool[] enemiesGameObjectPool;
        public Vector3[] enemiesPosition;
        public GameObjectPool[] GetEnemyGameObjectPool()
        {
            return (enemiesGameObjectPool);
        }
        // Plane
        [SerializeField] private GameObjectPool planeGameObjectPool;
        public Vector3[] planesPosition;
        public GameObjectPool GetplaneGameObjectPool()
        {
            return (planeGameObjectPool);
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
        SetPositionSpawnEnemy(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetPositionSpawnEnemy(int level)
    {
        //clear danh sách enemy
        ClearEnemies();

        for (int i = 0 ; i < levels[level].enemiesPosition.Length; i++)
        {
            foreach (Vector3 pos in levels[level].enemiesPosition)
            {
                //i is type enemy spawn
                StartCoroutine(SpawnEnemy(pos,level,i));
            }
        }
    }

    IEnumerator SpawnEnemy(Vector3 position,int level,int enemyType) {
        yield return new WaitForSeconds(delaySpawnEnemy);
        // objectPoolerManager.SpawnObject(levels[level].GetEnemyGameObjectPool(), position, Quaternion.identity);
    }

    private void ClearEnemies() {
        // gameManager.ClearEnemies();
    }
}
