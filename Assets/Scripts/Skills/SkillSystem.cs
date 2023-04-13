using System;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField] private EquipmentManager equipmentManager;
    public static int currentEnergy;
    public static int maxEnergy = 100;
    private PlayerData playerData;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
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
            Debug.Log(energy);
        }

    }
}
