using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private CharacterShooting shooting;
    private bool isActive;
    [SerializeField]
    private GameObject inventory;
    public void Start()
    {
        inventory.SetActive(false);
        var player = GameObject.FindGameObjectWithTag("Player");
        shooting = player.GetComponent<CharacterShooting>();
        isActive = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            shooting.enabled = !shooting.enabled;
            inventory.SetActive(!inventory.activeSelf);
            Cursor.visible = !Cursor.visible;
        }
    }
}
