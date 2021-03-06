﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraForLabirint : MonoBehaviour
{
    private GameObject cameraObj;
    private GameObject currentRoom;
    private GameObject player;
    private bool followCamera = true;
    private float cameraBoundsLeft;
    private float cameraBoundsRight;
    private float cameraBoundsUp;
    private float cameraBoundsDown;

    public static CameraForLabirint instance;

    private Camera cameraComponent;

    private void Awake()
    {
        instance = this;
        cameraObj = Camera.main.gameObject;
        player = GameObject.FindWithTag("Player");
        cameraComponent = cameraObj.GetComponent<Camera>();
    }

    private void Update()
    {
        if (followCamera && !CharacterLife.isDeath) 
            CameraFollowUpdate();        
    }

    public void ChangeRoom(GameObject room) {
        if (!followCamera)
            cameraObj.transform.position = room.transform.position + 20 * Vector3.back;
        else 
            CameraFollowSetup(room);
    }

    void CameraFollowSetup(GameObject room) {
        Dictionary<Direction.Side, float> borders = room.GetComponent<Room>().GetBordersFromTilemap();
        cameraBoundsLeft = borders[Direction.Side.LEFT];
        cameraBoundsRight = borders[Direction.Side.RIGHT];
        cameraBoundsUp = borders[Direction.Side.UP];
        cameraBoundsDown = borders[Direction.Side.DOWN];
    }

    void CameraFollowSetupOld(GameObject room) 
    {
        Tilemap[] tilemaps = room.GetComponentsInChildren<Tilemap>();
        float left, right, up, down;
        foreach (Tilemap tilemap in tilemaps) {
            if (tilemap.tag == "Environment") { // to separete layer with walls
                Vector3Int tilePosition;
                left = Mathf.Infinity;
                right = -Mathf.Infinity;
                up = -Mathf.Infinity;
                down = Mathf.Infinity;
                for (int x = tilemap.origin.x; x < tilemap.size.x; x++)
                {
                    for (int y = tilemap.origin.y; y < tilemap.size.y; y++)
                    {
                        tilePosition = new Vector3Int(x, y, 0);
                        if (tilemap.HasTile(tilePosition))
                        {
                            if (tilemap.CellToWorld(tilePosition).x < left) left = tilemap.CellToWorld(tilePosition).x;
                            if (tilemap.CellToWorld(tilePosition).x > right) right = tilemap.CellToWorld(tilePosition).x;
                            if (tilemap.CellToWorld(tilePosition).y < down) down = tilemap.CellToWorld(tilePosition).y;
                            if (tilemap.CellToWorld(tilePosition).y > up) up = tilemap.CellToWorld(tilePosition).y;
                        }
                    }
                }
                cameraBoundsLeft = left+1; //+1 to shift from bot-left corner of tile to center, mb need to replace with something tile size related later
                cameraBoundsRight = right+1;
                cameraBoundsUp = up+1;
                cameraBoundsDown = down+1;
            }
        }
    }

    void CameraFollowUpdate(){
        cameraObj.transform.position = player.transform.position - 20 * Vector3.forward;
        if (cameraComponent.ViewportToWorldPoint(Vector3.one).x - cameraComponent.ViewportToWorldPoint(Vector3.zero).x > cameraBoundsRight - cameraBoundsLeft)
        {
            cameraObj.transform.position = new Vector3((cameraBoundsRight + cameraBoundsLeft) / 2, cameraObj.transform.position.y, cameraObj.transform.position.z);
        }
        else
        {
            if (cameraComponent.ViewportToWorldPoint(Vector3.zero).x < cameraBoundsLeft)
                cameraObj.transform.position += Vector3.right * (cameraBoundsLeft - cameraComponent.ViewportToWorldPoint(Vector3.zero).x);
            if (cameraComponent.ViewportToWorldPoint(Vector3.one).x > cameraBoundsRight)
                cameraObj.transform.position += Vector3.right * (cameraBoundsRight - cameraComponent.ViewportToWorldPoint(Vector3.one).x);
        }

        if (cameraComponent.ViewportToWorldPoint(Vector3.one).y - cameraComponent.ViewportToWorldPoint(Vector3.zero).y > cameraBoundsUp - cameraBoundsDown)
        {
            cameraObj.transform.position = new Vector3(cameraObj.transform.position.x, (cameraBoundsUp + cameraBoundsDown) / 2,  cameraObj.transform.position.z);
        }
        else
        {
            if (cameraComponent.ViewportToWorldPoint(Vector3.one).y > cameraBoundsUp)
                cameraObj.transform.position += Vector3.up * (cameraBoundsUp - cameraComponent.ViewportToWorldPoint(Vector3.one).y);
            if (cameraComponent.ViewportToWorldPoint(Vector3.zero).y < cameraBoundsDown)
                cameraObj.transform.position += Vector3.up * (cameraBoundsDown - cameraComponent.ViewportToWorldPoint(Vector3.zero).y);
        }
    }
}
