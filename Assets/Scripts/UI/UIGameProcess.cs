using UnityEngine;
using UnityEngine.UI;

public class UIGameProcess : MonoBehaviour
{
    private Slider slider;

    public void CreateProcessBar(float maxProcess)
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxProcess;
        slider.value = 0;
    }

    public void UpdateProcessBar(float process)
    {
        slider.value += process;
    }

}
