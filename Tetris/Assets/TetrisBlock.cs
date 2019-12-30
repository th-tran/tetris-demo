using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    float previousTime;
    float fallTime = 0.8f;
    public Vector3 rotationPoint;
    enum Rotation {
        LEFT,
        RIGHT
    }
    public enum State {
        SPAWN,
        RIGHT,
        TWO,
        LEFT
    };
    public State currentState;
    public bool isSpecialBlock = false;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.SPAWN;
        if (rotationPoint != Vector3.zero)
        {
            isSpecialBlock = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!GameManager.Instance.ValidMove(transform))
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!GameManager.Instance.ValidMove(transform))
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) ||
                 Input.GetKeyDown(KeyCode.X))
        {
            AttemptRotation(Rotation.RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) ||
                 Input.GetKeyDown(KeyCode.Z))
        {
            AttemptRotation(Rotation.LEFT);
        }

        // Fall in increments of fallTime
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!GameManager.Instance.ValidMove(transform))
            {
                transform.position -= new Vector3(0, -1, 0);
                GameManager.Instance.AddToGrid(transform);
                GameManager.Instance.CheckForLines();
                this.enabled = false;
                TetrominoSpawner.Instance.NewTetromino();
            }
            previousTime = Time.time;
        }
    }

    void AttemptRotation(Rotation rotation)
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), (rotation == Rotation.RIGHT ? -90 : 90));
        if (!GameManager.Instance.ValidMove(transform))
        {
            GameManager.Instance.WallKick(transform);
            // Simplied rotation system: do not rotate if it collides with another block
            if (!GameManager.Instance.ValidMove(transform))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), (rotation == Rotation.RIGHT ? 90 : -90));
            }
        }
        else
        {
            // Update current block state
            switch (currentState) {
                case State.SPAWN:
                    currentState = (rotation == Rotation.RIGHT ? State.RIGHT : State.LEFT);
                    break;
                case State.RIGHT:
                    currentState = (rotation == Rotation.RIGHT ? State.TWO : State.SPAWN);
                    break;
                case State.TWO:
                    currentState = (rotation == Rotation.RIGHT ? State.LEFT : State.RIGHT);
                    break;
                case State.LEFT:
                    currentState = (rotation == Rotation.RIGHT ? State.SPAWN : State.TWO);
                    break;
                default:
                    break;
            }
        }
    }
}
