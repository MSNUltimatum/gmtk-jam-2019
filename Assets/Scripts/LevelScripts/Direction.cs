using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class Direction
{
    [Serializable]
    public enum Side
    {
        UNSET,
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
    
    /// Array that contains 4 sides to iterate on: down, left, right, up 
    public static readonly Side[] sides = new Side[] { Side.DOWN, Side.LEFT, Side.RIGHT, Side.UP };

    public static Side InvertSide(Side side)
    {
        switch (side)
        {
            case Side.UP:
                return Side.DOWN;
            case Side.RIGHT:
                return Side.LEFT;
            case Side.DOWN:
                return Side.UP;
            case Side.LEFT:
                return Side.RIGHT;
            default:
                Debug.LogError("Trying to invert unknown side");
                return Side.DOWN;
        }
    }

    public static Vector3 SideToVector3(Side side)
    {
        switch (side)
        {
            case Side.UP:
                return Vector3.up;
            case Side.RIGHT:
                return Vector3.right;
            case Side.DOWN:
                return Vector3.down;
            case Side.LEFT:
                return Vector3.left;
            default:
                return Vector3.zero;
        }
    }
}
