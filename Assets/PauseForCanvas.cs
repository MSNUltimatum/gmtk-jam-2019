using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseForCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject pauseCanvas = null;
    void Start()
    {
        Instantiate(pauseCanvas);
    }
}
