using UnityEngine;

public class BonousCoin : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
    }

    public void Bonousx3Coin(bool isTriple)
    {
        gameManager.BonousCoin(isTriple);
    }
    
}
