using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable() {
        gameManager.ResumeGame();
    }

    private void OnDisable() {
        Debug.Log("OnDisable");
    }

    private void OnDestroy() {
        Debug.Log("OnDestroy");
    }
}
