using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject windowPopup;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable() {
        gameManager.OnBuyFailure += ShowPopup;
    }

    private void OnDisable() {
        gameManager.OnBuyFailure -= ShowPopup;
        
    }
    
    private void ShowPopup()
    {
        windowPopup.SetActive(true);
    }
}
