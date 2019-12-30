using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    // Game boudaries
    public static int height = 20;
    public static int width = 10;
    public static Transform[,] grid;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
         grid = new Transform[width, height];
    }

    public bool ValidMove(Transform transform)
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
            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }

    public void WallKick(Transform transform)
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

    public void AddToGrid(Transform tetrisBlock)
    {
        foreach (Transform block in tetrisBlock)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);

            grid[roundedX, roundedY] = block;
        }
    }

    public void CheckForLines()
    {
        for (int y = height-1; y >= 0; y--)
        {
            if (HasLine(y))
            {
                DeleteLine(y);
                MoveRowDown(y);
            }
        }
    }

    bool HasLine(int y)
    {
        // Check for a full line
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    void MoveRowDown(int y)
    {
        for (int newY = y; newY < height; newY++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, newY] != null)
                {
                    // Move block down
                    grid[x, newY-1] = grid[x, newY];
                    grid[x, newY] = null;
                    // Move grid down relatively
                    grid[x, newY-1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
}
