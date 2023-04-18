using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public GameObject hitCount;
    public GameObject totalHit;
    public TMP_Text pointTxt, totalHitTxt, totalComboTxt, textPointTxt, coinTxt, moneyStartTxt;
    private PlayerData playerData;
    private Animator animator;
    private int previousHash;
    private int startHash;
    private int startGameHash;
    private int winGameHash;
    private int loseGameHash;
    private int hitPoint;
    private int totalHitPoint;
    [SerializeField] private Slider countHitComboTimer;
    private bool flagCountHit;
    private SoundManager soundManager;
    private SettingData settingData;
    public Slider processGame;
    public TextMeshProUGUI processPrecent;
    public LevelState levelState;
    [SerializeField] private Image characterAva;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image[] skillBtn;
    [SerializeField] private TextMeshProUGUI[] skillCountTxt;
    [SerializeField] private TextMeshProUGUI energyTxt;
    [SerializeField] private TextMeshProUGUI cooldownTxt;
    [SerializeField] private Slider healthBarPlayer;

    private AbsPlayerSkill[] absPlayerSkills;

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

    //Audio
    public AudioClip clickBtn;
    public AudioClip backgroundSound;
    public AudioClip winSound;
    public AudioClip loseSound;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        previousHash = Animator.StringToHash("isPrevious");
        startGameHash = Animator.StringToHash("StartGame");
        startHash = Animator.StringToHash("Start");
        winGameHash = Animator.StringToHash("WinGame");
        loseGameHash = Animator.StringToHash("LoseGame");
        soundManager = SoundManager.Instance;
        gameManager = GameManager.Instance;
        settingData = SettingData.Load();
    }

    private void OnEnable()
    {
        gameManager.OnStartGame += StartGame;
        gameManager.OnEndGame += HandleEndGameUI;
        gameManager.OnNotEnoughEnergy += DisplayNotEnoughEnergy;
        PlayerCharacter playerCharacter = gameManager.DisplayHeroInUi();
        characterAva.sprite = playerCharacter.thumbnail;
        characterName.text = playerCharacter.name;
        for (int i = 0; i < playerCharacter.skillStates.Length; i++)
        {
            skillBtn[i].sprite = playerCharacter.skillStates[i].sprite;
        }

    }

    private void OnDisable()
    {
        gameManager.OnStartGame -= StartGame;
        gameManager.OnEndGame -= HandleEndGameUI;
        gameManager.OnNotEnoughEnergy -= DisplayNotEnoughEnergy;
    }

    void Start()
    {
        soundManager.MuteGame(settingData.mute);
        playerData = PlayerData.Load();
        CoefficientCombo = 0.5f;
        bigSize = 150;
        smallSize = 120;
        timerBigSize = 0.2f;
        soundManager.SetMusicbackGround(backgroundSound);
        soundManager.SetPlayMusic(true);
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
        absPlayerSkills = gameManager.player.GetComponent<PlayerController>().absPlayerSkills;
    }

    private void Update()
    {
        for (int i = 0; i < absPlayerSkills.Length; i++)
        {
            if (absPlayerSkills[i].cooldownTimer != 0)
            {
                skillCountTxt[i].text = Mathf.Round(absPlayerSkills[i].cooldownTimer)+"";
            }
        }
        // Debug.Log(gameManager.player.GetComponent<PlayerDamageable>().maxHealth);
    }

    //Display Hit Point in UI
    public void DisplayHitPoint(bool hit)
    {
        flagCountHit = hit;
        if (hit && !totalHit.activeInHierarchy)
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
        if (win)
        {
            BonusCoin(levelState.bonousCoin);
            animator.SetTrigger(winGameHash);
            soundManager.PlaySound(winSound);
        }
        else
        {
            animator.SetTrigger(loseGameHash);
            soundManager.PlaySound(loseSound);
        }
    }

    private void BonusCoin(int coin)
    {
        coinTxt.text = coin + "";
    }

    //Used
    public void BonusCoin(bool isTriple)
    {
        gameManager.BonousCoin(isTriple, levelState.bonousCoin);
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

    private void DisplayNotEnoughEnergy()
    {
        energyTxt.gameObject.SetActive(true);
        Invoke("HideNotEnoughEnergy", 2f);
    }

    private void DisplayWaitForCoolDown()
    {
        cooldownTxt.gameObject.SetActive(true);
    }

    private void HideNotEnoughEnergy()
    {
        energyTxt.gameObject.SetActive(false);
    }

    public void PlaySound()
    {
        soundManager.PlaySound(clickBtn);
    }

}
