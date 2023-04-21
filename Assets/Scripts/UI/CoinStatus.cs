using TMPro;
using UnityEngine;

public class CoinStatus : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coin;
    private PlayerData playerData;

    private void OnEnable() {
        playerData = PlayerData.Load();
        coin.text = playerData.money +"";
    }
}
