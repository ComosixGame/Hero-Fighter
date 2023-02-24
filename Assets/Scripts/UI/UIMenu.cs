using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class UIMenu : MonoBehaviour
{
    public GameObject hitCount;
    public TMP_Text pointTxt, hitTxt;

    // Start is called before the first frame update
    void Start()
    {
        DisplayHitPoint();
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
            pointTxt.fontSize = 120;
            pointTxt.text = "1";
            hitTxt.text = "Hit";
        // }
        
            StartCoroutine(DisplayHit());
    }

    IEnumerator DisplayHit() {
        yield return new WaitForSeconds(10f);
        pointTxt.fontSize = 40;
    }

}
