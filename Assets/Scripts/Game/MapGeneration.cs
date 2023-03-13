using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGeneration : MonoBehaviour
{

    [Header("List of enemy wave")]
    public EnemyWave[] enemyWaves;
    private int currentWave = 0;
    private int countEnemyDeath = 0;
    private GameManager gameManager;
    public CinemachineConfinerController cinemachineConfinerController;

    private void Awake()
    {
        gameManager = GameManager.Instance;
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
        countEnemyDeath= enemyWaves[currentWave].EnemyList.Count;
        StartNewWave();
    }

    //Able all the enemies in wave
    private void StartNewWave()
    {
        for (int i = 0; i < enemyWaves.Length; i++)
        {
            EnemyWave wave = enemyWaves[i];
            bool isCurrentWave = i == currentWave;
            foreach (GameObject enemy in wave.EnemyList)
            {
                enemy.SetActive(isCurrentWave);
            }
        }
    }

    public void NextWave()
    {
        currentWave++;
        StartNewWave();
        countEnemyDeath= enemyWaves[currentWave].EnemyList.Count;
        cinemachineConfinerController.ChangeConfiner(currentWave);
    }

    private void EnemyDeath()
    {
        
        if (countEnemyDeath != 0)
        {
            countEnemyDeath--;
            if(countEnemyDeath == 0)
            {
                BoxCollider areaCollider = enemyWaves[currentWave].AreaCollider;
                if (areaCollider != null)
                areaCollider.isTrigger = true;
            }
        }
    }

   
}



//EnemyWave class
[System.Serializable]
public class EnemyWave
{
    public string WaveName = "Wave";
    public BoxCollider AreaCollider; //a collider that keeps the player from leaving an area
    public List<GameObject> EnemyList = new List<GameObject>();

}
