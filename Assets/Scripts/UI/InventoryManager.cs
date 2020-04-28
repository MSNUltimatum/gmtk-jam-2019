using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private CharacterShooting shooting;
    [SerializeField]
    public GameObject inventory = null;
    public void Start()
    {
        inventory.SetActive(false);
        var player = GameObject.FindGameObjectWithTag("Player");
        shooting = player.GetComponent<CharacterShooting>();
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
