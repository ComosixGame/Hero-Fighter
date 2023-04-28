using TMPro;
using UnityEngine;

public class CoinStatus : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coin;
    private PlayerData playerData;
    private GameManager gameManager;
    
    private void Awake() {
        gameManager = GameManager.Instance;
    }

    private void OnEnable() {
        gameManager.OnUpdateMoney += UpdateMoney;
        gameManager.OnBuyHero += UpdateMoneyHero;
        gameManager.OnBuy += UpdateMoneySkill;
        playerData = PlayerData.Load();
        coin.text = playerData.money +"";
    }

    private void UpdateMoney(int money)
    {
        playerData = PlayerData.Load();
        coin.text = playerData.money + "";
    }

    private void UpdateMoneyHero()
    {
        playerData = PlayerData.Load();
        coin.text = playerData.money + "";
    }

    private void UpdateMoneySkill()
    {
        playerData = PlayerData.Load();
        coin.text = playerData.money + "";
    }

    private void OnDisable() {
        gameManager.OnUpdateMoney -= UpdateMoney;
        gameManager.OnBuyHero -= UpdateMoneyHero;
        gameManager.OnBuy -= UpdateMoneySkill;
    }
}
