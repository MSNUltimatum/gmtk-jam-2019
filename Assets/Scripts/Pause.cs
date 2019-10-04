using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    public static bool Paused { get; private set; } = false;

    public static bool UnPaused { get { return !Paused; } }

    private void Start()
    {
        Paused = false;
        myTransform = transform;
        ChangeMenuVisibility();
    }

    private static void ChangeMenuVisibility()
    {
        for (int i = 0; i < myTransform.childCount; i++)
        {
            myTransform.GetChild(i).gameObject.SetActive(Paused);
        }
    }

    public static void SetPause(bool shouldPause, bool openMenu = true)
    {
        Paused = shouldPause;

        if (openMenu) ChangeMenuVisibility();
    }

    public void ResumeGame()
    {
        SetPause(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetPause(!Paused);
        }
    }

    private static Transform myTransform;
}
