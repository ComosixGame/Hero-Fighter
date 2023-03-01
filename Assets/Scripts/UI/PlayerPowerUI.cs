using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerUI : MonoBehaviour
{
    private Slider slider;
    private float powerUsingSkill;

    public void CreatePowerBar(float maxPower, float powerUsingSkill)
    {
        slider = GetComponentInChildren<Slider>();
        this.powerUsingSkill = powerUsingSkill;
        slider.maxValue = maxPower;
        slider.value = 0;
    }

    public void UpdatePowerBarValue(float power)
    {
        slider.value += power;
    }

    public void UsePower(float power)
    {
        if (slider.value >= this.powerUsingSkill)
        {
            //Using Skill
            slider.value -= power;
        }else
        {
            //Do Not Power Using Skill
        }
    }
}
