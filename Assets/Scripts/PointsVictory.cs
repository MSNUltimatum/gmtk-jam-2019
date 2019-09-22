using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsVictory : RelodScene
{
    [SerializeField]
    int pointsToVictory;

    protected override void Victory()
    {
        if (TotalValue == pointsToVictory)
        {
            isVictory = true;
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
