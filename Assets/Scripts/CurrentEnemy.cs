using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject CanvasPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        var canvasEnemyName = Instantiate(CanvasPrefab);
        EnemyName = canvasEnemyName.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        gameController = GameObject.FindGameObjectWithTag("GameController");
        roomLighting = gameController.GetComponent<RoomLighting>();
        scenesController = gameController.GetComponent<RelodScene>();
    }

    public static void SetCurrentEnemy(string enemyName, GameObject enemy, bool killed = true)
    {
        EnemyName.text = enemyName;

        if (killed)
        {
            if (scenesController)
            {
                scenesController.CurrentCount(1);
            }
            roomLighting.Lighten(1);
        }
        
    }

    private static RoomLighting roomLighting;
    private static RelodScene scenesController;
    private GameObject gameController;
    private static TMPro.TextMeshProUGUI EnemyName;
}
