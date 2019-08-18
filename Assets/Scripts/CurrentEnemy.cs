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
        // UI BLOCK
        var canvas = Instantiate(CanvasPrefab);
        EnemyName = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        // LOGIC BLOCK
    }

    public void SetCurrentEnemy(string enemyName, GameObject enemy)
    {
        // UI BLOCK
        EnemyName.text = enemyName;
        // LOGIC BLOCK
    }

    private TMPro.TextMeshProUGUI EnemyName;
}
