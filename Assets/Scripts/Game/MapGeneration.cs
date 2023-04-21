using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private bool debug;
    public LevelState levelState;
    [SerializeField] private UIMenu uIMenu;
    [SerializeField] private Collider[] areaColliders;
    [SerializeField] private Collider[] wallColliders;
    private int currentWave = 0;
    private int totalWaves;
    private GameManager gameManager;
    private ObjectPoolerManager objectPooler;
    private List<GameObjectPool> enemyList = new List<GameObjectPool>();
    private bool isStart, isReset;
    private PlayerData playerData;
    private float process;
    private float totalEnemy;
    private void Awake()
    {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPoolerManager.Instance;
    }

    private void OnEnable()
    {
        isStart = false;
        isReset = false;
        levelState = gameManager.levelState;
        uIMenu.processGame.value = 0;
        uIMenu.processPrecent.text = "0%";
        for (int i = 0; i < levelState.waves.Count; i++)
        {
            totalEnemy += levelState.waves[i].enemies.Count;
        }
        process = 1 / totalEnemy;

        for (int i = 0; i < areaColliders.Length; i++)
        {
            areaColliders[i].transform.position = new Vector3(levelState.areaRestrictors[i].x, levelState.areaRestrictors[i].y, levelState.areaRestrictors[i].z);
        }

        for (int i = 0; i < wallColliders.Length; i++)
        {
            wallColliders[i].transform.position = new Vector3(levelState.wallColliders[i].x, levelState.wallColliders[i].y, levelState.wallColliders[i].z);
        }
    }

    private void Start()
    {
        totalWaves = levelState.waves.Count;
        StartNewWave();
    }

    private void Update()
    {
        if (enemyList != null)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].gameObject.GetComponent<EnemyDamageable>().destroyed)
                {
                    uIMenu.processGame.value += process;
                    uIMenu.processPrecent.text = Mathf.Round(uIMenu.processGame.value * 100) + "%";
                    enemyList.RemoveAt(i);
                }
            }
        }


        if (currentWave < totalWaves - 1)
        {
            if (enemyList.Count == 0)
            {
                //event
                uIMenu.PreviousAnimation(true);
                areaColliders[currentWave].isTrigger = true;
                currentWave++;
                StartNewWave();
            }
        }
        else
        {
            if (enemyList.Count == 0)
            {
                gameManager.GameWin();
                isStart = false;
            }
        }
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
                Gizmos.color = new Color32(255, 0, 0, 200);
                Gizmos.DrawSphere(enemy.position, 1.2f);
            }
        }
    }

#endif
}