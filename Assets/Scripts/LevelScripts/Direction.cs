using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction
{
    public enum Side
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }

    public static Side[] Sides()
    {
        return new Side[] { Side.DOWN, Side.LEFT, Side.RIGHT, Side.UP };
    }

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
                return Side.UP;
        }
    }
}
