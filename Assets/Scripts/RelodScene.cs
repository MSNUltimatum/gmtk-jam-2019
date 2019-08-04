using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RelodScene : MonoBehaviour
{
    private ArenaEnemySpawner arena;
    private float TotalValue = 0;
    private float maxvalue = 0;
    [SerializeField]
    private string NextSceneName = "";
    private GameObject Canvas;
    private void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        arena = GetComponent<ArenaEnemySpawner>();
        Canvas.transform.GetChild(0).gameObject.SetActive(false);
        maxvalue = arena.EnemyCount();
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
            Canvas.transform.GetChild(0).gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                Canvas.transform.GetChild(0).gameObject.SetActive(false);
                SceneManager.LoadScene(NextSceneName);
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {

            TotalValue = 0;
            Time.timeScale = 1;
            Canvas.transform.GetChild(1).gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
    }
    public void PressR()
    {
        Canvas.transform.GetChild(1).gameObject.SetActive(true);
    }
}
