using UnityEngine;
using UnityEngine.UI;

public class GameProcessBar : MonoBehaviour
{
    private Slider slider;

    public void CreateProcessGameBar(float maxProcess)
    {
        slider = GetComponentInChildren<Slider>();
        slider.maxValue = maxProcess;
        slider.value    = 0;
    }

   public void UpdateProcessGameBarValue(float process)
   {
        slider.value += process;
   }
}
