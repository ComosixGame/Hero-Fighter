using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private bool debug;
    public LevelState levelState;
    [SerializeField] private UIMenu uIMenu;
    [SerializeField] private Collider[] areaColliders;
    private int currentWave = 0;
    private int totalWaves;
    private GameManager gameManager;
    private ObjectPoolerManager objectPooler;
    private List<GameObjectPool> enemyList = new List<GameObjectPool>();
    private bool isStart, isReset;
    private PlayerData playerData;
    private void Awake()
    {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPoolerManager.Instance;
    }

    private void OnEnable()
    {
        isStart = false;
        isReset = false;
        gameManager.OnStartGame += StartGame;
        gameManager.OnNewGame += ResetGame;
        if(!debug) {
            levelState = gameManager.levelState;
        }
    }

    private void OnDisable()
    {
        gameManager.OnStartGame -= StartGame;
        gameManager.OnNewGame -= ResetGame;
    }

    private void Update()
    {
        if (!isReset)
        {
            if (enemyList != null)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList[i].gameObject.GetComponent<EnemyDamageable>().destroyed)
                    {
                        enemyList.RemoveAt(i);
                    }
                }
            }
        }


        if(isStart)
        {
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
                    isStart = false;
                }
            }
        }
    }


    private void StartGame()
    {
        totalWaves = levelState.waves.Count;
        StartNewWave();
    }

    private void ResetGame()
    {
        isReset = true;
    }

    //Able all the enemies in wave
    private void StartNewWave()
    {
        isStart = true;
        for (int i = 0; i < levelState.waves.Count; i++)
        {
            LevelState.Wave wave = levelState.waves[i];
            bool isCurrentWave = i == currentWave;
            foreach (LevelState.Enemy enemy in wave.enemies)
            {
                if (isCurrentWave)
                {
                    GameObjectPool gameObjectPool = objectPooler.SpawnObject(enemy.enemyObjectPool, enemy.position, enemy.rotation);
                    enemyList.Add(gameObjectPool);
                }
            }
        }
    }

    public void NextWave()
    {
        currentWave++;
        StartNewWave();
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