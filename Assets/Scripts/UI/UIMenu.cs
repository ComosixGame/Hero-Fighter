using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public GameObject hitCount;
    public GameObject perivous;
    public TMP_Text pointTxt, hitTxt, moneyTxt, moneyStartTxt;
    private PlayerData playerData;
    private Animator animator;
    public GameObject playUI;
    public GameObject winUI;
    public GameObject loseUI;
    private int previousHash;
    private int hitPoint;
    [SerializeField] private Slider countHitComboTimer;
    private bool flagCountHit;
    private int money;
    private int totalMoney;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.Load();
        CoefficientCombo = 0.5f;
        bigSize = 150;
        smallSize = 100;
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
        }

    }


    //Display Hit Point in UI
    public void DisplayHitPoint(bool hit)
    {
        flagCountHit = hit;
        if (hit)
        {
            hitCount.SetActive(true);
            hitPoint++;
            pointTxt.text = hitPoint + "";
            pointTxt.fontSize = bigSize;
            StartCoroutine(SetFontSizeInit(timerBigSize));
            countHitComboTimer.value = 1;
        }
        else
        {
            hitCount.SetActive(false);
            hitPoint = 0;
        }
    }

    //Set font Size 
    IEnumerator SetFontSizeInit(float time)
    {
        yield return new WaitForSeconds(time);
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
