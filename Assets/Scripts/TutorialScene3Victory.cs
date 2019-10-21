using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TutorialScene3Victory : RelodScene
{
    [SerializeField]
    private float timeToVictory = 25.0f;

    public float TimeToVictory { get { return timeToVictory; } }

    protected override void Awake()
    {
        base.Awake();
        light = GetComponent<RoomLighting>();
        light.SetMaxValue(timeToVictory);
    }
    protected override void Victory()
    {
        timeToVictory -= Time.deltaTime;
        if (timeToVictory < 0)
        {
            TutorialScript3 tut = GetComponent<TutorialScript3>();
            tut.isVictoryT = true;
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
        else
        {
            light.AddToLight(Time.deltaTime / 8);
        }
    }
    private RoomLighting light;
}