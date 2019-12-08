using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenuAfterTimeout : MonoBehaviour
{
    [SerializeField]
    private float TimeToMenu = 20f;
    private float TimeLeft = 1;

    // Start is called before the first frame update
    void Start()
    {
        TimeLeft = TimeToMenu;
    }

    // Update is called once per frame
    void Update()
    {
        TimeLeft -= Time.deltaTime;
        if (TimeLeft < 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
