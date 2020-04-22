using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    private bool laserDidHit = false;
    private GameObject player;
    [SerializeField] private float laserWight = 0.1f;
    
    private Vector3 laserStartPos;
    private Vector3 laserEndPos;

    private void Awake()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        if (line == null) Debug.LogError("Laser can't find LineRenderer");
        else line.enabled = false;
        player = GameObject.FindWithTag("Player");
    }

    public void ShootStart(Vector3 fromPosition, Vector3 toPosition) {
        line.enabled = true;
        line.positionCount = 2;
        line.SetPosition(0, fromPosition);
        line.SetPosition(1, toPosition);
        line.startWidth = laserWight;
        line.endWidth = laserWight;
        laserDidHit = false;
        laserStartPos = fromPosition;
        laserEndPos = toPosition;
    }

    public void ShootStop()
    {
        line.enabled = false;
    }

    private void Update()
    {
        if (line.enabled && !Pause.Paused && !laserDidHit) {
            if (PlayerInTheRay())
            {
                player.GetComponent<CharacterLife>().Damage(); ;
                laserDidHit = true;
            }
        }
    }

    private bool PlayerInTheRay()
    {
        bool result = false;
        RaycastHit2D[] hitArray = Physics2D.BoxCastAll(laserStartPos, new Vector2(laserWight,laserWight), 0, laserEndPos - laserStartPos, Vector3.Distance(laserEndPos, laserStartPos));
        foreach (RaycastHit2D hit in hitArray) {
            if (hit.collider.gameObject.tag == "Player") result = true;
        }
        return result; 
    }
}
