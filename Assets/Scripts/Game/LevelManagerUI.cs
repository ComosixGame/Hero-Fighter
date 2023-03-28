using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerUI : MonoBehaviour
{
    public Transform contentLevel;
    public LevelCard cardLevel;
    public Vector3 cardOriginPosition;

    // Start is called before the first frame update
    void Start()
    {
        LevelCard lv = Instantiate(cardLevel, cardOriginPosition, Quaternion.identity);
        lv.transform.SetParent(contentLevel, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
