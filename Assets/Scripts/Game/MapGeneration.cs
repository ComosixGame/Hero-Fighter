using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public LevelState levelState;
    [SerializeField] private UIMenu uIMenu;
    [SerializeField] private Collider[] areaColliders;
    [SerializeField] private CinemachineConfinerController cinemachineConfinerController;
    private int currentWave = 0;
    private int countEnemyDeath = 0;
    private int totalWaves;
    private GameManager gameManager;
    private ObjectPoolerManager objectPooler;
    private List<GameObjectPool> enemyList = new List<GameObjectPool>();

    private void Awake()
    {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPoolerManager.Instance;
        enemyList.Clear();
    }

    private void OnEnable()
    {
        gameManager.OnStartGame += StartGame;
        gameManager.OnEnemyDeath += EnemyDeath;
        uIMenu = FindObjectOfType<UIMenu>();
    }

    private void OnDisable()
    {
        gameManager.OnStartGame -= StartGame;
        gameManager.OnEnemyDeath -= EnemyDeath;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (enemyList != null)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].gameObject.GetComponent<EnemyDamageable>().destroyed )
                {
                    enemyList.RemoveAt(i);
                }
            }

        } 
        if (currentWave < totalWaves - 1)
        {
            if (enemyList.Count == 0)
            {
                uIMenu.PreviousAnimation(true);
                areaColliders[currentWave].isTrigger = true;
            }
        } else
        {
            if (enemyList.Count == 0)
            {
                gameManager.GameWin();
            }
        }
    }

    private void StartGame()
    {
        countEnemyDeath = levelState.waves[currentWave].enemies.Count;
        totalWaves = levelState.waves.Count;
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
                if (isCurrentWave)
                {
                    GameObjectPool gop = objectPooler.SpawnObject(enemy.enemyObjectPool, enemy.position, enemy.rotation);
                    enemyList.Add(gop);
                }
            }
        }
    }

    public void NextWave()
    {
        currentWave++;
        StartNewWave();
        // countEnemyDeath = levelState.waves[currentWave].enemies.Count;
        // cinemachineConfinerController.ChangeConfiner(currentWave);
    }

    private void EnemyDeath()
    {
        if (currentWave < totalWaves - 1)
        {
            if (countEnemyDeath != 0)
            {
                countEnemyDeath--;
                if (countEnemyDeath == 0)
                {
                    uIMenu.PreviousAnimation(true);
                    areaColliders[currentWave].isTrigger = true;
                }
            }
        }
        else
        {
            if (countEnemyDeath != 0)
            {
                countEnemyDeath--;
                if (countEnemyDeath == 0)
                {
                    gameManager.GameWin();
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
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