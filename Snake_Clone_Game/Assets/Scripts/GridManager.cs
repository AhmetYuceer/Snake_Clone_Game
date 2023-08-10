using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private bool DebugMod;

    public static float gridX = 14f;
    public static float gridY = 14f;
    static public bool[,] grid = new bool[14,14];

    [SerializeField] private GameObject gridPrefabs;
    [SerializeField] private GameObject gridParent;

    private List<GameObject> gridObject = new List<GameObject>();
    static public List<Vector2> snakePos = new List<Vector2>(); 

    private void Start() 
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                grid[x, y] = false;
            }
        }
        if (DebugMod)
        {
            CreateGridObject();
        }
    }
    private void Update() 
    {
        gridControl();
    }

    private void gridControl()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                grid[x, y] = false;
            }
        }
        foreach (var pos in snakePos)
        {
            if (pos.x != 14 && pos.x != -1)
            {
                if (pos.y != 14 && pos.y != -1)
                {
                    grid[(int)pos.x, (int)pos.y] = true;       
                }
                else
                {
                    SnakeController.isDead =true;
                }
            }
            else
            {
                SnakeController.isDead =true;
            }
        }
        gridColor();
    }

    private void gridColor()
    {
      if (DebugMod)
      {
          int say = 0;
          foreach (var item in grid)
          {
              if (item)
                  gridObject[say].GetComponent<SpriteRenderer>().color = Color.green;
              else
                  gridObject[say].GetComponent<SpriteRenderer>().color = Color.white;
              say++;
          }
      }
    }
    private void CreateGridObject()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int a = 0; a < gridY; a++)
            {
                GameObject clone =  Instantiate(gridPrefabs,gridParent.transform);
                clone.transform.position = new Vector2(i,a);
                gridObject.Add(clone);
            }
        }
    }
}