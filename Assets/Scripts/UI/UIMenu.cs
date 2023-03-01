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
    private int previousHash;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        previousHash = Animator.StringToHash("isPrevious");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Test
        DisplayHitPoint();
        playerData = PlayerData.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Display Hit Point in UI
    private void DisplayHitPoint()
    {
        hitCount.SetActive(true);
        // if ()
        // {
        //Set Size 
            pointTxt.fontSize = 120;
            pointTxt.text = "1";
            hitTxt.text = "Hit";
        // }
        
            StartCoroutine(DisplayHitReturnSizeInit());
    }

    //Return Size Initi
    IEnumerator DisplayHitReturnSizeInit() {
        yield return new WaitForSeconds(10f);
        pointTxt.fontSize = 40;
    }

    //True if Clear Wave and False if Player Move to new Wave
    public void PreviousAnimation(bool isActive)
    {
        animator.SetBool(previousHash, isActive);
    }

}