using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentEnemyUI : MonoBehaviour
{
    [SerializeField]
    GameObject CanvasPrefab = null;

    // Start is called before the first frame update
    void Awake()
    {
        var canvasEnemyName = Instantiate(CanvasPrefab);
        var canvasScript = canvasEnemyName.GetComponent<Canvas>();
        canvasScript.worldCamera = Camera.main;
        canvasScript.sortingLayerName = "OnEffect";
        EnemyName = canvasEnemyName.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        enemyNameUICenter = canvasEnemyName.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
    }

    public static void SetCurrentEnemy(GameObject enemy)
    {
        var text = enemy.GetComponent<TMPro.TextMeshPro>().text;
        SetCurrentEnemy(text);
    }

    public static void SetCurrentEnemy(string enemyName)
    {
        var UIwidth = Mathf.Lerp(80, 200, (enemyName.Length - 5) / 8f);
        enemyNameUICenter.sizeDelta = new Vector2(UIwidth, 80);
        EnemyName.text = enemyName;
    }

    private GameObject gameController;
    private static RectTransform enemyNameUICenter = null;
    public static TMPro.TextMeshProUGUI EnemyName;
}
