using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyNewYearHat : MonoBehaviour
{
    [SerializeField]
    private GameObject newYearHat = null;
    [SerializeField]
    private int fromDecember = 14;
    [SerializeField]
    private int toJanuary = 7;
    
    // Start is called before the first frame update
    void Start()
    {
        var timeNow = System.DateTime.Now;
        if (newYearHat != null)
        {
            if (timeNow.Month == 12)
            {
                 newYearHat.SetActive(timeNow.Day >= fromDecember ? true : false);
            }
            else if (timeNow.Month == 1)
            {
                newYearHat.SetActive(timeNow.Day <= toJanuary ? true : false);
            }
        }
        
    }
}
