using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private GameObject Canvas;
    void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 1;
            Canvas.transform.GetChild(0).gameObject.SetActive(false);
            Canvas.transform.GetChild(1).gameObject.SetActive(false);
            Canvas.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
