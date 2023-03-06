using System.Collections;
using UnityEngine;
using TMPro;

public class UIMenu : MonoBehaviour
{
    public GameObject hitCount;
    public GameObject perivous;
    public TMP_Text pointTxt, hitTxt;
    private PlayerData playerData;
    private Animator animator;
    public GameObject playUI;
    public GameObject winUI;
    public GameObject loseUI;
    private int previousHash;
    private int hitPoint;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        previousHash = Animator.StringToHash("isPrevious");
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Display Hit Point in UI
    public void DisplayHitPoint()
    {
        hitCount.SetActive(true);
        // if ()
        // {
        //Set Size 
            pointTxt.fontSize = 120;
            hitPoint++;
            pointTxt.text = hitPoint + "";
            hitTxt.text = "Hit";
        // }
        
            StartCoroutine(DisplayHitReturnSizeInit(10));
    }

    //Return Size Initi
    IEnumerator DisplayHitReturnSizeInit(float time) {
        yield return new WaitForSeconds(time);
        pointTxt.fontSize = 40;
    }

    //True if Clear Wave and False if Player Move to new Wave
    public void PreviousAnimation(bool isActive)
    {
        animator.SetBool(previousHash, isActive);
    }

    public void GameWin(){
        playUI.SetActive(false);
        winUI.SetActive(true);
    }


    public void GameLose(){
        playUI.SetActive(false);
        loseUI.SetActive(true);
    }
}
