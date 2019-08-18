using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public List<string> Scenes = new List<string>();

    public void ClickButtonContinue()
    {
        //PlayerPrefs.DeleteAll();
        //Debug.Log(PlayerPrefs.GetInt("CurrentScene"));
        if (PlayerPrefs.HasKey("CurrentScene"))
        {
            SceneManager.LoadScene(Scenes[PlayerPrefs.GetInt("CurrentScene")]);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentScene", 0);
            SceneManager.LoadScene(Scenes[0]);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
