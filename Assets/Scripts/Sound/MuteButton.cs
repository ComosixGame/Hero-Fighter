using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public bool isMute;
    private SettingData settingData;
    private Button button;
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        settingData = SettingData.Load();
        OnMute(settingData.mute);

        soundManager.OnMute += OnMute;

        button.onClick.AddListener(OnClick);
    }

    private void OnMute(bool mute)
    {
        if (mute)
        {
            gameObject.SetActive(!isMute);
        }
        else
        {
            gameObject.SetActive(isMute);
        }

    }

    private void OnClick()
    {
        soundManager.MuteGame(isMute);
    }

    private void OnDestroy()
    {
        soundManager.OnMute -= OnMute;
    }
}
