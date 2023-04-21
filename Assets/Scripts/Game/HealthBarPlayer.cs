using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarPlayer : MonoBehaviour
{
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        PlayerDamageable.onTakeDamage += UpdateSlider;
        PlayerDamageable.onInit += Init;
    }

    private void OnDisable()
    {
        PlayerDamageable.onTakeDamage -= UpdateSlider;
        PlayerDamageable.onInit -= Init;
    }



    private void Init(float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value =  maxValue;
    }

    private void UpdateSlider(float value)
    {
        slider.value = value;
    }
}
