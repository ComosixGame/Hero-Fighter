using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public GameObject hitCount;
    public GameObject totalHit;
    public TMP_Text pointTxt, totalHitTxt, totalComboTxt, textPointTxt, moneyTxt, moneyStartTxt;
    private PlayerData playerData;
    private Animator animator;
    public GameObject playUI;
    public GameObject winUI;
    public GameObject loseUI;
    private int previousHash;
    private int startHash;
    private int startGameHash;
    private int hitPoint;
    private int totalHitPoint;
    [SerializeField] private Slider countHitComboTimer;
    private bool flagCountHit;
    private int money;
    private int totalMoney;
    private SoundManager soundManager;
    private SettingData settingData;

    //Coefficient Count Hit Combo
    private float CoefficientCombo;

    //Font size big display when player attack
    private float bigSize;

    //Font size small display
    private float smallSize;

    //Time display big size
    private float timerBigSize;

    //Color 
    [SerializeField] private Color color, color1, color2, color3, color4;
    private GameManager gameManager;

    private void Awake() {
        animator = GetComponent<Animator>();
        previousHash = Animator.StringToHash("isPrevious");
        startGameHash = Animator.StringToHash("StartGame");
        startHash = Animator.StringToHash("Start");
        soundManager = SoundManager.Instance;
        gameManager = GameManager.Instance;
        settingData = SettingData.Load();
    }

    private void OnEnable() {
        gameManager.OnStartGame += StartGame;
        gameManager.OnEndGame += HandleEndGameUI;
    }

    private void OnDisable() {
        gameManager.OnStartGame -= StartGame;
        gameManager.OnEndGame -= HandleEndGameUI;
    }

    void Start()
    {
        soundManager.MuteGame(settingData.mute);
        playerData = PlayerData.Load();
        CoefficientCombo = 0.5f;
        bigSize = 150;
        smallSize = 120;
        timerBigSize = 0.2f;
    }

    void FixedUpdate()
    {
        if (flagCountHit)
        {
            countHitComboTimer.value -= Time.deltaTime * CoefficientCombo;
        }

        if (countHitComboTimer.value <= 0 && !totalHit.activeInHierarchy)
        {
            DisplayHitPoint(false);
            Invoke("DisplayTotalHit", 0.5f);
        }
    }

    private void StartGame()
    {
        animator.SetTrigger(startGameHash);
    }

    //Display Hit Point in UI
    public void DisplayHitPoint(bool hit)
    {
        flagCountHit = hit;
        if (hit)
        {
            hitCount?.SetActive(true);
            totalHit?.SetActive(false);
            hitPoint++;
            pointTxt.text = hitPoint + "";
            pointTxt.fontSize = bigSize;
            Invoke("SetFontSizeInit", timerBigSize);
            countHitComboTimer.value = 1;
            totalHitPoint = hitPoint;
            
        }
        else
        {
            hitCount?.SetActive(false);
            hitPoint = 0;
            Invoke("DisplayTotalHit", 0.5f);
        }

    }

    private void DisplayTotalHit()
    {
        if (totalHitPoint != 0 && !totalHit.activeInHierarchy)
        {
            totalHit?.SetActive(true);
            hitCount?.SetActive(false);
            Color currentColor;
            string textPointText;
            if (totalHitPoint <= 2)
            {
                currentColor = color;
                textPointText = "GOOD";
            }
            else if (totalHitPoint <= 4)
            {
                currentColor = color1;
                textPointText = "NICE";
            }
            else if (totalHitPoint <= 8)
            {
                currentColor = color2;
                textPointText = "EXCELLENT";
            }
            else if (totalHitPoint <= 20)
            {
                currentColor = color3;
                textPointText = "LEGENDRY";
            }
            else
            {
                currentColor = color4;
                textPointText = "GODLIKE";
            }
            textPointTxt.color = currentColor;
            totalHitTxt.color = currentColor;
            totalComboTxt.color = currentColor;
            textPointTxt.text = textPointText;
            totalHitTxt.text = totalHitPoint + "";
            Invoke("DisplayTotalHitDone", 0.5f);
        }
    }

    private void DisplayTotalHitDone()
    {
        totalHit?.SetActive(false);
        hitCount?.SetActive(false);
        totalHitPoint = 0;
    }

    //Set font size init
    private void SetFontSizeInit()
    {
        pointTxt.fontSize = smallSize;
    }

    //True if Clear Wave and False if Player Move to new Wave
    public void PreviousAnimation(bool isActive)
    {
        animator.SetBool(previousHash, isActive);
    }

    public void PauseGame()
    {
        gameManager.PauseGame();
    }

    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }

    private void HandleEndGameUI(bool win)
    {
        if(win) {
            BonusMoney();
            winUI.SetActive(true);
        } else {
            loseUI.SetActive(true);
        }
    }

    private void BonusMoney()
    {
        money = Random.Range(150, 210);
        moneyTxt.text = money + "";
    }

    public void BonusX3Money()
    {
        money = money * 3;
        SaveMoney();
    }

    public void SaveMoney()
    {
        playerData = PlayerData.Load();
        totalMoney = playerData.money;
        totalMoney += money;
        playerData.money = totalMoney;
        playerData.Save();
    }

    public void LoadMoney()
    {
        playerData = PlayerData.Load();
        Debug.Log(playerData.money);
        moneyStartTxt.text = playerData.money + "";
    }

    //Used
    public void SaveLevel(bool isSave)
    {
        gameManager.NewGame(isSave);
    }

    public void ReadyFight()
    {
        gameManager.InitUiDone();
    }

}
