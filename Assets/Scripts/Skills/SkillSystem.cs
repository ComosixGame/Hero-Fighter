using System;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField] private EquipmentManager equipmentManager;
    public static int currentEnergy = 20;
    public static int maxEnergy = 100;
    private PlayerData playerData;
    private GameManager gameManager;
    public EnergyBarPlayer energyBarPlayer;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        energyBarPlayer = FindObjectOfType<EnergyBarPlayer>();
        energyBarPlayer?.CreateEnergyBar(currentEnergy, maxEnergy);
    }

    private void OnEnable()
    {
        PlayerHurtBox.OnHit += BonusEnergy;
    }

    private void OnDisable()
    {
        PlayerHurtBox.OnHit += BonusEnergy;
    }

    private void Start() {
        
    }

    private void BonusEnergy(int energy)
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energy;
        }

    }
}
