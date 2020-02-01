using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MetricsRecords
{
    public bool[] levelComlpeted;
    public string[] levelSceneName;
    public float[] levelTime;
    public int[] deathCount;

    public MetricsRecords() {
        int max = SceneManager.sceneCountInBuildSettings; 
        
        levelComlpeted = new bool[max+1];//number is from scene count
        levelSceneName = new string[max+1];
        levelTime = new float[max+1];
        deathCount = new int[max+1];
        for (int i = 0; i < max; i++) {
            levelComlpeted[i] = false;
            levelSceneName[i] = "";
            levelTime[i] = 0;
            deathCount[i] = 0;
        }
    }

}
