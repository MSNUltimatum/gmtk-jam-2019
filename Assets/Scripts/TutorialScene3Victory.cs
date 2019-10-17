using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TutorialScene3Victory : RelodScene
{
    [SerializeField]
    private float timeToVictory = 25.0f;
    protected override void Victory()
    {
        timeToVictory -= Time.deltaTime;
        if(timeToVictory > 0)
            Canvas.transform.GetChild(3).GetComponent<Text>().text = timeToVictory.ToString();
        else
            Canvas.transform.GetChild(3).GetComponent<Text>().text = "0";
        if (timeToVictory < 0)
        {
            TutorialScript3 tut = GetComponent<TutorialScript3>();
            tut.isVictory = true;
            Canvas.transform.GetChild(0).gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) && !CharacterLife.isDeath)
            {
                if (PlayerPrefs.GetInt("CurrentScene") < SceneNumber + 1)
                    PlayerPrefs.SetInt("CurrentScene", SceneNumber + 1);
                Canvas.transform.GetChild(0).gameObject.SetActive(false);
                SceneManager.LoadScene(NextSceneName);
            }
        }
    }
}
