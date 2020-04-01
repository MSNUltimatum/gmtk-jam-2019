using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private CharacterShooting shooting;
    private bool isActive;
    public void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        var player = GameObject.FindGameObjectWithTag("Player");
        shooting = player.GetComponent<CharacterShooting>();
        isActive = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            shooting.enabled = !shooting.enabled;
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
            Cursor.visible = !Cursor.visible;
        }
    }
}
