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
        EnemyName.GetComponent<Canvas>().sortingLayerName = "OnEffect";
        enemyNameUICenter = canvasEnemyName.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
    }

    private void Update()
    {
        timeSinceLastNewName = Mathf.Clamp01(timeSinceLastNewName + (Time.deltaTime / transitionTime));
        EnemyName.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, timeSinceLastNewName));
        enemyNameUICenter.sizeDelta = new Vector2(Mathf.Lerp(oldUIwidth, newUIwidth, timeSinceLastNewName), 80);
    }

    public static void SetCurrentEnemy(GameObject enemy)
    {
        var text = enemy.GetComponent<TMPro.TextMeshPro>().text;
        SetCurrentEnemy(text);
    }

    public static void SetCurrentEnemy(string enemyName)
    {
        timeSinceLastNewName = 0;
        oldUIwidth = enemyNameUICenter.sizeDelta.x;
        newUIwidth = Mathf.Lerp(20, 120, (enemyName.Length - 5) / 8f);
        
        EnemyName.text = enemyName;
    }

    private const float transitionTime = 0.35f;
    private static float timeSinceLastNewName = 1;
    private static float newUIwidth = 40;
    private static float oldUIwidth = 40;
    private static float newNameOpacity = 1;

    private GameObject gameController;
    private static RectTransform enemyNameUICenter = null;
    public static TMPro.TextMeshProUGUI EnemyName;
}
