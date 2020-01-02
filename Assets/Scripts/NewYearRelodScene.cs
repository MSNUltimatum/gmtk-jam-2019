using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewYearRelodScene : RelodScene
{
    [SerializeField]
    private TMPro.TextMeshProUGUI hatsCollected = null;

    public override void UpdateScore(int val)
    {
        base.UpdateScore(val);
        hatsCollected.text = TotalValue.ToString();
    }
}
