using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject TitleScreen = null;
    private TitleScreenContainer titleScreenContainer;

    // Start is called before the first frame update
    void Start()
    {
        // УБЕРИ ПОЖАЛУЙСТА НУ ПРОШУ ТЕБЯ НУ ТЫ ЧО
        PlayerPrefs.SetInt("CurrentScene", -1);
        ActivateTitleScreen();
        GetScenesInBuild();
    }

    void ActivateTitleScreen()
    {
        TitleScreen.SetActive(true);
        titleScreenContainer = TitleScreen.GetComponent<TitleScreenContainer>();
        GrayLoadNotPlayedYet();
    }

    void GetScenesInBuild()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string[] scenesInBuild = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            scenesInBuild[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        scenes = scenesInBuild;
    }

    void GrayLoadNotPlayedYet()
    {
        if (!PlayerPrefs.HasKey("CurrentScene") || PlayerPrefs.GetInt("CurrentScene") == -1)
        {
            PlayerPrefs.SetInt("CurrentScene", 0);
            var btn = titleScreenContainer.GetButtonContinue();
            var btnImage = btn.GetComponent<Image>();
            btnImage.color = new Color(0.37f, 0.37f, 0.37f); // gray
            btn.GetComponent<Button>().enabled = false;
        }
    }

    private string[] scenes = null;
}
