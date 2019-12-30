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

    [SerializeField]
    private GameObject SettingsScreen = null;

    [SerializeField]
    private GameObject StageSelectionScreen = null;

    [SerializeField]
    private GameObject Credits = null;

    [SerializeField]
    private GameObject Chapter1 = null;

    [SerializeField]
    private GameObject DiffictultyLabel = null;

    [SerializeField]
    private Sprite Chapter1EasyDiff = null;
    [SerializeField]
    private Sprite Chapter1NormalDiff = null;
    [SerializeField]
    private Sprite Chapter1HardcoreDiff = null;

    enum Difficulty
    {
        Easy,
        Normal,
        Hardcore
    }
    private Difficulty stageDifficulty = Difficulty.Normal;
    

    #region Monobehaviour functions

    void Start()
    {
        SetActiveTitle(true);
        GetScenesInBuild();
        creditsStartPosition = Credits.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateEverything();
            SetActiveTitle(true);
        }
    }

    #endregion

    #region Technical Functions
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
        // TODO ВРЕМЕННЫЙ ФИКС
        if (!PlayerPrefs.HasKey("CurrentScene") || PlayerPrefs.GetInt("CurrentScene") == -1)
        {
            PlayerPrefs.SetInt("CurrentScene", -1);
            var btn = titleScreenContainer.GetButtonContinue();
            var btnImage = btn.GetComponent<Image>();
            btnImage.color = new Color(0.37f, 0.37f, 0.37f); // gray
            btn.GetComponent<Button>().enabled = false;
        }
    }

    private void DeactivateEverything()
    {
        SetActiveTitle(false);
        SetActiveSettings(false);
        SetActiveStageSelection(false);
        SetActiveCredits(false);
        SetActiveChapter1(false);
    }

    #endregion

    #region Public Functions

    // old and simple
    public void NewGame()
    {
        switch (stageDifficulty)
        {
            case Difficulty.Easy:
                SceneManager.LoadScene("EasyTutorialScene1");
                break;
            case Difficulty.Normal:
                SceneManager.LoadScene("TutorialScene");
                break;
            case Difficulty.Hardcore:
                SceneManager.LoadScene("HardTutorialScene");
                break;
            default:
                break;
        }
        
    }

    public void NewYear()
    {
        SceneManager.LoadScene("NewYearLevel");
    }

    public void ClickButtonSettings()
    {
        DeactivateEverything();
        SetActiveSettings(true);
    }

    public void ClickButtonLoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentScene"));
    }

    // moves to the stage selection screen
    public void ClickButtonNewGame()
    {
        DeactivateEverything();
        SetActiveStageSelection(true);
    }

    public void ClickButtonCredits()
    {
        SetActiveCredits(true);
    }

    public void ClickButtonChapter1()
    {
        DeactivateEverything();
        SetActiveChapter1(true);
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("CurrentScene", -1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ClickButtonDifficultyLeft()
    {
        switch (stageDifficulty)
        {
            case Difficulty.Easy:
                break;
            case Difficulty.Normal:
                stageDifficulty = Difficulty.Easy;
                DiffictultyLabel.GetComponent<Image>().sprite = Chapter1EasyDiff;
                break;
            case Difficulty.Hardcore:
                stageDifficulty = Difficulty.Normal;
                DiffictultyLabel.GetComponent<Image>().sprite = Chapter1NormalDiff;
                break;
            default:
                break;
        }
    }

    public void ClickButtonDifficultyRight()
    {
        switch (stageDifficulty)
        {
            case Difficulty.Easy:
                stageDifficulty = Difficulty.Normal;
                DiffictultyLabel.GetComponent<Image>().sprite = Chapter1NormalDiff;
                break;
            case Difficulty.Normal:
                stageDifficulty = Difficulty.Hardcore;
                DiffictultyLabel.GetComponent<Image>().sprite = Chapter1HardcoreDiff;
                break;
            case Difficulty.Hardcore:
                break;
            default:
                break;
        }
    }

    #endregion

    #region Screen Layour Activator Functions

    private void SetActiveTitle(bool active = true)
    {
        if (active)
        {
            TitleScreen.SetActive(true);
            titleScreenContainer = TitleScreen.GetComponent<TitleScreenContainer>();
            GrayLoadNotPlayedYet();
        }
        else
        {
            TitleScreen.SetActive(false);
        }
    }

    private void SetActiveSettings(bool active = true)
    {
        SettingsScreen.SetActive(active);
    }

    private void SetActiveStageSelection(bool active = true)
    {
        StageSelectionScreen.SetActive(active);
    }

    private void SetActiveCredits(bool active = true)
    {
        Credits.transform.position = creditsStartPosition;
        Credits.SetActive(active);
    }

    private void SetActiveChapter1(bool active = true)
    {
        Chapter1.SetActive(active);
    }

    #endregion

    private string[] scenes = null;
    private Vector3 creditsStartPosition;
}
