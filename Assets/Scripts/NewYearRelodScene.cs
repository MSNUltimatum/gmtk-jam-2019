using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewYearRelodScene : RelodScene
{
    [SerializeField]
    private TMPro.TextMeshProUGUI hatsCollected = null;

    public override void CurrentCount(int val)
    {
        base.CurrentCount(val);
        hatsCollected.text = TotalValue.ToString();
    }
}
