using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
#if UNITY_EDITOR
[CanEditMultipleObjects]
#endif
public class HealthBarRennder
{
    public GameObject healthBar;
    public float offset;
    private Camera camera;
    private GameObject _healthBar;
    private Slider sliderHealthBar;

    public void CreateHealthBar(Transform parent ,float Maxhealth) {
        camera = Camera.main;
        _healthBar = GameObject.Instantiate(healthBar);
        _healthBar.transform.SetParent(parent,false);
        _healthBar.transform.position = parent.position + Vector3.up * offset;
        sliderHealthBar = _healthBar.GetComponentInChildren<Slider>();
        sliderHealthBar.maxValue = Maxhealth;
        sliderHealthBar.value = Maxhealth;
    }

    public void UpdateHealthBarRotation() {;
        Vector3 dirCam = camera.transform.position - _healthBar.transform.position;
        dirCam.x = 0;
        _healthBar.transform.rotation = Quaternion.LookRotation(dirCam.normalized);
    }

    public void UpdateHealthBarValue(float health) {
        sliderHealthBar.value = health;
    }

}
