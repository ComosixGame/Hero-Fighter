using UnityEngine;
using UnityEngine.UI;

public class HealthBarPlayer : MonoBehaviour
{
    private Slider slider;

    public void CreateHealthBar(float maxHealth)
    {
        slider = GetComponentInChildren<Slider>();
        slider.maxValue = maxHealth;
        slider.value    = maxHealth;
    }

   public void UpdateHealthBarValue(float health)
   {
        slider.value = health;
   }
}
