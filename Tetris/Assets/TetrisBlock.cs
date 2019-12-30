using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    float previousTime;
    float fallTime = 0.8f;
    public Vector3 rotationPoint;

    // Start is called before the first frame update
    void Start()
    {

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
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            if (!GameManager.Instance.ValidMove(transform))
            {
                GameManager.Instance.WallKick(transform);
            }
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
}
