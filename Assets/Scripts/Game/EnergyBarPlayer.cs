using UnityEngine;
using UnityEngine.UI;

public class EnergyBarPlayer : MonoBehaviour
{
    private Slider slider;
    private float maxEnergy;
    public void CreateEnergyBar(float currentEnergy, float maxEnergy)
    {
        slider = GetComponentInChildren<Slider>();
        slider.maxValue = maxEnergy;
        slider.value    = currentEnergy;
        this.maxEnergy = maxEnergy;
    }

    public void UpdateHealthBarValue(float energy)
    {
        if (energy < maxEnergy)
        {
            slider.value += energy;
        }
    }
}
