using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICurrentEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject CanvasPrefab;

    // Start is called before the first frame update
    void Start()
    {
        var canvas = Instantiate(CanvasPrefab);
        EnemyName = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void SetCurrentEnemy(string enemyName)
    {
        EnemyName.text = enemyName;
    }

    private TMPro.TextMeshProUGUI EnemyName;
}
