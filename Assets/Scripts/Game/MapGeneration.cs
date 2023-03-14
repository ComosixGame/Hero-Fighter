using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public LevelState levelState;
    [SerializeField] private Collider[] areaColliders;
    [SerializeField] private CinemachineConfinerController cinemachineConfinerController;
    private int currentWave = 0;
    private int countEnemyDeath = 0;
    private GameManager gameManager;
    private ObjectPoolerManager objectPooler;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPoolerManager.Instance;
    }

    private void OnEnable()
    {
        gameManager.OnStartGame += StartGame;
        gameManager.OnEnemyDeath += EnemyDeath;
    }

    private void OnDisable()
    {
        gameManager.OnStartGame -= StartGame;
        gameManager.OnEnemyDeath -= EnemyDeath;
    }

    private void Start()
    {
        
    }

    private void StartGame()
    {
        countEnemyDeath= levelState.waves[currentWave].enemies.Count;
        StartNewWave();
    }

    //Able all the enemies in wave
    private void StartNewWave()
    {
        for (int i = 0; i < levelState.waves.Count; i++)
        {
            LevelState.Wave wave = levelState.waves[i];
            bool isCurrentWave = i == currentWave;
            foreach (LevelState.Enemy enemy in wave.enemies)
            {
                if(isCurrentWave) {
                    objectPooler.SpawnObject(enemy.enemyObjectPool, enemy.position, enemy.rotation);
                } else {
                    objectPooler.DeactiveObject(enemy.enemyObjectPool);
                }
            }
        }
    }

    public void NextWave()
    {
        currentWave++;
        StartNewWave();
        countEnemyDeath= levelState.waves[currentWave].enemies.Count;
        cinemachineConfinerController.ChangeConfiner(currentWave);
    }

    private void EnemyDeath()
    {
        
        if (countEnemyDeath != 0)
        {
            countEnemyDeath--;
            if(countEnemyDeath == 0)
            {
                areaColliders[currentWave].isTrigger = true;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        foreach (LevelState.Wave wave in levelState.waves)
        {
            foreach (LevelState.Enemy enemy in wave.enemies)
            {
                GameObject prefab = enemy.enemyObjectPool.GetGameObject();
                Gizmos.color = new Color32(255, 0, 0, 255);
                Gizmos.DrawMesh(prefab.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh, enemy.position, Quaternion.Euler(enemy.eulerRotation));
            }
        }
    }

#endif
}