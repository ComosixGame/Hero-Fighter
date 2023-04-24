using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image backgroundCoolDown;
    [SerializeField] private Image backgroundEnergy;

    [SerializeField] private TextMeshProUGUI coolDownTime;
    private Transform player;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        StartGameEvent.OnStart += StartGame;
    }

    private void StartGame()
    {
        player = gameManager.player;
        player.GetComponent<PlayerController>().playerSkills[id].OnCooldownTimer += UpdateCoolDown;
        player.GetComponent<PlayerController>().playerSkills[id].OnCooldownTimerDone += UpdateCoolDownDone;
        player.GetComponent<PlayerController>().playerSkills[id].OnNotEnoughEnergy += NotEnoughEnergy;
        player.GetComponent<PlayerController>().playerSkills[id].OnEnoughEnergy += EnoughEnergy;
    }

    private void UpdateCoolDown(float time)
    {
        coolDownTime.text = Mathf.Round(time) + "";
        backgroundCoolDown.gameObject.SetActive(true);
    }

    private void UpdateCoolDownDone()
    {
        coolDownTime.text = "";
        backgroundCoolDown.gameObject.SetActive(false);
    }

    private void NotEnoughEnergy()
    {
        backgroundEnergy.gameObject.SetActive(true);
    }

    private void EnoughEnergy()
    {
        backgroundEnergy.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StartGameEvent.OnStart -= StartGame;
        player.GetComponent<PlayerController>().playerSkills[id].OnCooldownTimer -= UpdateCoolDown;
        player.GetComponent<PlayerController>().playerSkills[id].OnCooldownTimerDone += UpdateCoolDownDone;
        player.GetComponent<PlayerController>().playerSkills[id].OnNotEnoughEnergy -= NotEnoughEnergy;
        player.GetComponent<PlayerController>().playerSkills[id].OnEnoughEnergy -= EnoughEnergy;
    }
}
