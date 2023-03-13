using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class CinemachineConfinerController : MonoBehaviour
{
    [SerializeField] private List<Collider> confiner;
    private CinemachineConfiner cinemachineConfiner;
    private GameManager gameManager;
    
    private void Awake()
    {
        gameManager = GameManager.Instance;
        cinemachineConfiner = GetComponent<CinemachineConfiner>();
    }

    private void OnEnable() 
    {
        gameManager.OnStartGame += StartGame;
    }
    private void OnDisable() 
    {
        gameManager.OnStartGame += StartGame;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartGame()
    {
        cinemachineConfiner.m_BoundingVolume = confiner[0];
    }

    public void ChangeConfiner(int wave)
    {

        for (int i = 0; i< confiner.Count; i++)
        {
            if (i != wave)
            {
                confiner[i].gameObject.SetActive(false);
            }
            else
            {
                confiner[i].gameObject.SetActive(true);
                cinemachineConfiner.m_BoundingVolume = confiner[wave];
            }
        }
    }
}
