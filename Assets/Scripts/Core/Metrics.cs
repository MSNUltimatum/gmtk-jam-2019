using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class Metrics : MonoBehaviour
{
    [SerializeField]
    private static MetricsRecords metrics = null;

    private static string fileName = "metrics.bin";
    private static int sceneIndex;
    private static bool levelIsRuning = true;

    private void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (metrics == null) {
            LoadMetrics(out metrics);
        }
        metrics.levelSceneName[sceneIndex] = SceneManager.GetActiveScene().name;        
    }

    private void Update()
    {
        if (!Pause.Paused && levelIsRuning) {
            metrics.levelTime[sceneIndex] += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            OutputMetrics();
        }
    }

    private void OnDestroy()
    {
        SaveMetrics(metrics);// to update time on exit to main menu or closing the game
    }

    public static void OnNewGame() {
        metrics = new MetricsRecords();  //to empty arrays
        SaveMetrics(metrics); // overwrite the save
    }

    public static void OnContineuGame() {
        LoadMetrics(out metrics);
    }

    public static void OnWin() { 
        metrics.levelComlpeted[sceneIndex] = true;
        SaveMetrics(metrics);
    }

    public static void OnDeath() {
        metrics.deathCount[sceneIndex]++;
        SaveMetrics(metrics); 
    }

    static private void SaveMetrics(MetricsRecords data)
    {
        BinaryFormatter binaryformatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + fileName);
        binaryformatter.Serialize(file, data);

        file.Close();
    }

    private static void LoadMetrics(out MetricsRecords data) {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter binaryformatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            data = (MetricsRecords)binaryformatter.Deserialize(file);

            file.Close();
        }
        else {
            data = new MetricsRecords();
            SaveMetrics(data); // saving empty file
        }
    }

    private static void OutputMetrics() {
        Debug.Log( "metrics output");
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            if (metrics.levelComlpeted[i]) { // for finished levels
                Debug.Log(i.ToString() + " " + metrics.levelSceneName[i] + " death:" + metrics.deathCount[i] + " time:" + metrics.levelTime[i]);
            }
        }
        if (!metrics.levelComlpeted[sceneIndex]) { //for current scene
            Debug.Log(sceneIndex.ToString() + " " + metrics.levelSceneName[sceneIndex] + " death:" + metrics.deathCount[sceneIndex] + " time:" + metrics.levelTime[sceneIndex] + " unfinished");
        }
    }

}
