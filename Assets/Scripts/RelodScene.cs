using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RelodScene : MonoBehaviour
{
    private ArenaEnemySpawner arena;
    private float TotalValue = 0;
    private float maxvalue = 0;

    private void Start()
    {
        arena = GetComponent<ArenaEnemySpawner>();
        maxvalue = arena.EnemyCount();
        Debug.Log(maxvalue);
    }

    public void CurrentCount(int val)
    {
        TotalValue = TotalValue + val;
        Debug.Log(TotalValue);
    }

    private void Update()
    {
        if(TotalValue == maxvalue)
        {
            SceneManager.LoadScene("NewMsnScene");
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            TotalValue = 0;
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
    }
}
