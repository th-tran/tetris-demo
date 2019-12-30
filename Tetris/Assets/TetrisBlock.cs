using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    float previousTime;
    float fallTime = 0.8f;
    public Vector3 rotationPoint;
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
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AttemptRotation();
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

    void AttemptRotation()
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        if (!GameManager.Instance.ValidMove(transform))
        {
            GameManager.Instance.WallKick(transform);
        }
        // Update current block state
        // TODO: Implement and account for left rotation
        switch (currentState) {
            case State.SPAWN:
                currentState = State.RIGHT;
                break;
            case State.RIGHT:
                currentState = State.TWO;
                break;
            case State.TWO:
                currentState = State.LEFT;
                break;
            case State.LEFT:
                currentState = State.SPAWN;
                break;
            default:
                break;
        }
    }
}
