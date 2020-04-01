using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeFrame : MonoBehaviour
{
    public static void Frame(GameObject cell, Sprite frame)
    {
        cell.GetComponent<Image>().sprite = frame;
    }
}
