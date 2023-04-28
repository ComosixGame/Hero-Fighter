using System;
using System.Collections;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField] private EquipmentManager equipmentManager;
    [SerializeField] private int maxEnergy;
    private int currentEnergy = 0;
    private PlayerData playerData;
    private GameManager gameManager;
    public static event Action<int> onUpdatEnegry;
    public static event Action<int> onInit;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        onInit?.Invoke(maxEnergy);
    }

    private void Start() {
        UpdateEnergy(maxEnergy);
    }

    private void OnEnable()
    {
        currentEnergy = 0;
        PlayerHurtBox.OnHit += UpdateEnergy;
    }

    private void OnDisable()
    {
        PlayerHurtBox.OnHit -= UpdateEnergy;
    }
    
    public void UpdateEnergy(int energy) {

        currentEnergy += energy;
        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
        }

        if(currentEnergy <= 0) {
            currentEnergy = 0;
        }
        
        onUpdatEnegry?.Invoke(currentEnergy);
    }

    public bool CheckEnergy(int energy) {
        return currentEnergy >= energy;
    }
}
