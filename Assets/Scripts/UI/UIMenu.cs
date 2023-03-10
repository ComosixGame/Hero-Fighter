using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public GameObject hitCount;
    public GameObject totalHit;
    public TMP_Text pointTxt, totalHitTxt, textPointTxt,moneyTxt, moneyStartTxt;
    private PlayerData playerData;
    private Animator animator;
    public GameObject playUI;
    public GameObject winUI;
    public GameObject loseUI;
    private int previousHash;
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

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        previousHash = Animator.StringToHash("isPrevious");
        soundManager = SoundManager.Instance;
        settingData = SettingData.Load();
    }

    // Start is called before the first frame update
    void Start()
    {
        soundManager.MuteGame(settingData.mute);
        playerData = PlayerData.Load();
        CoefficientCombo = 0.5f;
        bigSize = 150;
        smallSize = 80;
        timerBigSize = 0.2f;
    }

    void FixedUpdate()
    {
        if(flagCountHit)
        {
            countHitComboTimer.value -= Time.deltaTime*CoefficientCombo;
        }

        if (countHitComboTimer.value <= 0)
        {
            DisplayHitPoint(false);
            Invoke("DisplayTotalHit", 0.5f);
        }

    }


    //Display Hit Point in UI
    public void DisplayHitPoint(bool hit)
    {
        flagCountHit = hit;
        if (hit)
        {
            hitCount.SetActive(true);
            if (pointTxt.color.a < 1)
            {
                Color hitalpha = pointTxt.color;
                hitalpha.a +=0.1f;
            }
            hitPoint++;
            pointTxt.text = hitPoint+" HIT";
            pointTxt.fontSize = bigSize;
            Invoke("SetFontSizeInit", timerBigSize);
            countHitComboTimer.value = 1;
            totalHitPoint = hitPoint;
        }
        else
        {
            hitCount.SetActive(false);
            hitPoint = 0;
            Invoke("DisplayTotalHit", 0.5f);
        }
    }

    private void DisplayTotalHit()
    {
        if (totalHitPoint != 0)
        {
            totalHit.SetActive(true);
            if (totalHitPoint <= 2)
            {
                totalHitTxt.text = totalHitPoint + "HIT";
            }
            else if (totalHitPoint <= 4)
            {
                textPointTxt.text = "Good";
                totalHitTxt.text = totalHitPoint + "HIT";
            }else if (totalHitPoint <= 8)
            {
                textPointTxt.text = "Excellent";
                totalHitTxt.text = totalHitPoint + "HIT";
            }
            else
            {
                textPointTxt.text = "Legendry";
                totalHitTxt.text = "Legendry" + totalHitPoint + "HIT";
            }
            Invoke("DisplayTotalHitDone", 0.5f);
        }
    }

    private void DisplayTotalHitDone()
    {
        totalHit.SetActive(false);
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

    public void GameWin(){
        BonusMoney();
        playUI.SetActive(false);
        winUI.SetActive(true);
    }


    public void GameLose(){
        playUI.SetActive(false);
        loseUI.SetActive(true);
    }

    private void BonusMoney()
    {
        money = Random.Range(150, 210);
        moneyTxt.text = money + "";
    }

    public void BonusX3Money()
    {
        money = money*3;
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
}
