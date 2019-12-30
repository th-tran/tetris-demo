using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    float previousTime;
    float fallTime = 0.8f;
    public Vector3 rotationPoint;
    // Game boundaries
    public static int height = 20;
    public static int width = 10;
    static Transform[,] grid = new Transform[width, height];

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
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            if (!ValidMove())
            {
                WallKick();
            }
        }

        // Fall in increments of fallTime
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
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

    bool ValidMove()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);

            // Check for out of bounds
            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            // Check for collision with other tetris blocks
            if (GameManager.grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }

    void WallKick()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);

            if (roundedX < 0)
            {
                transform.position += new Vector3(1, 0, 0);
            }
            else if (roundedX >= width)
            {
                transform.position += new Vector3(-1, 0, 0);
            }

            if (roundedY < 0)
            {
                transform.position += new Vector3(0, 1, 0);
            }
            else if (roundedY >= height)
            {
                transform.position += new Vector3(0, -1, 0);
            }
        }
    }
}
