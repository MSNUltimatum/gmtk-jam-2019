using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentEnemySelector : MonoBehaviour
{
    public GameObject currentBoy { get; private set; } = null;

    public bool enableScanning = true;

    private void Start()
    {
        timeToNextScan = timeToEachScan;

        currentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // Listen "Enemy is dead" events. If someone dies we immediately select a new enemy
        MonsterLife.OnEnemyDead += SelectRandomEnemy;
    }

    private void Update()
    {
        // If current enemy is far away by a certain proximity check
        if (!ProximitySuccess(currentBoy))
        {
            timeToNextScan = Mathf.Max(0, timeToNextScan - Time.deltaTime);
            if (timeToNextScan <= 0)
            {
                timeToNextScan = timeToEachScan;
                SelectRandomEnemy();
            }
        }
    }

    public void SelectRandomEnemy()
    {
        if (currentBoy != null)
        {
            currentBoy.GetComponent<MonsterLife>().MakeNoBoy();
            currentBoy = null;
        }

        ScanEnemiesInCloseProximity();
        if (enemiesOnScreen.Count != 0)
        {
            var theEnemy = enemiesOnScreen[Random.Range(0, enemiesOnScreen.Count)];
            SelectEnemy(theEnemy);
        }
        else CurrentEnemyUI.SetCurrentEnemy("");
    }

    public void SelectEnemy(GameObject theEnemy)
    {
        currentBoy = theEnemy;
        theEnemy.GetComponent<MonsterLife>().MakeBoy();
        CurrentEnemyUI.SetCurrentEnemy(theEnemy.GetComponentInChildren<TMPro.TextMeshPro>().text);
    }

    // Result is applied to enemiesOnScreen field
    private void ScanEnemiesInCloseProximity()
    {
        enemiesOnScreen = new List<GameObject>();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (ProximitySuccess(enemy) && enemy.GetComponent<MonsterLife>().HP > 0) enemiesOnScreen.Add(enemy);
        }
    }

    protected virtual bool ProximitySuccess(GameObject enemy)
    {
        if (enemy == null) return false;
        if (currentCamera == null)
        {
            print("haha");
            currentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        var enemyInScreenSpace = currentCamera.WorldToViewportPoint(enemy.transform.position);
        return enemyInScreenSpace.x >= 0 && enemyInScreenSpace.x <= 1 && enemyInScreenSpace.y >= 0 && enemyInScreenSpace.y <= 1;
    }

    private float timeToNextScan = float.PositiveInfinity;
    private float timeToEachScan = 0.25f;

    private List<GameObject> enemiesOnScreen = new List<GameObject>();
    private Camera currentCamera;
}
