using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLife : MonoBehaviour
{
    [SerializeField]
    private int HP = 1;

    private GameObject game;
    private RoomLighting Room;
    bool THE_BOY = false;
    
    private void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController");
        Room = game.GetComponent<RoomLighting>();
    }

    public void Damage()
    {
        if (THE_BOY)
        {
            HP--;
            GameObject.Find("Game Manager").GetComponent<ArenaEnemySpawner>().ChangeTheBoy(gameObject);
            // TODO: add EXPLOSION, MOTHERF!$&*ER
            if (HP == 0)
            {
                Room.Lighten(1);
                Destroy(gameObject);
            }
        }
        else
        {
            // TODO: make visual and sound effects of absorb
        }
    }

    public void MakeBoy()
    {
        THE_BOY = true;
    }
}
