using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ManaBar : MonoBehaviour
{
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        SkillSystem.onUpdatEnegry += UpdateSlider;
        SkillSystem.onInit += Init;
    }

    private void OnDisable()
    {
        SkillSystem.onUpdatEnegry -= UpdateSlider;
        SkillSystem.onInit -= Init;
    }



    private void Init(int maxValue)
    {
        slider.maxValue = maxValue;
    }

    private void UpdateSlider(int value)
    {
        slider.value = value;
    }
}
