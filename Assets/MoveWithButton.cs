using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithButton : MonoBehaviour
{
    public Direction dir;
    public float horizontal;
    private void Awake()
    {
        switch (dir)
        {
            case Direction.Left:
            horizontal = -1;
            break;
            case Direction.right:
            horizontal = 1;
            break;
        }

    }

    public enum Direction { Left, right }
}
