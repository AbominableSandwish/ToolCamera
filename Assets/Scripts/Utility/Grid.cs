using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Grid : MonoBehaviour
{
    private Transform target;
    private const int DELTA_SIZE = 10;

    private Vector2 gridWorldSize;
    private float nodeRadius =  1.0f;
    private float distance = 1.0f;

    Node[,] grid;
    public List<Node> FinalPath;

     public Node[,] GetGride()
     {
        return grid;
     }

    float nodeDiameter;

    private int gridSizeX, gridSizeY;

    private int columns;
    private int rows;

    public int GetRows()
    {
        return this.rows;
    }

    public int GetColumns()
    {
        return this.columns;
    }

    public void CreateGrid(int witdh, int height, Transform camera)
    {
        gridSizeX = witdh;
        gridSizeY = height;
        target = camera;

        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = target.position - Vector3.right * distance - Vector3.up * distance + Vector3.forward * 10;
        for(int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + distance) + Vector3.up * (y * nodeDiameter + distance);
                bool Wall = true;
                int[,] tiles = new int[(int)gridWorldSize.x, (int)gridWorldSize.y];

                grid[x, y] = new Node(worldPoint, x, y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(target != null)
        Gizmos.DrawSphere(target.position, nodeRadius);

        
        
        if(grid != null)
        {
            foreach(Node n in grid)
            {

                Gizmos.color = Color.red;

                if (n.isVisited)
                {
                    Gizmos.color = Color.cyan;
                }

                if(FinalPath != null)
                {
                    if (FinalPath.Contains(n))
                    {
                        Gizmos.color = Color.red;
                    }
                }

                Gizmos.DrawSphere(n.Position, nodeRadius / DELTA_SIZE);
            }
        }
        else
        {
            gridWorldSize = new Vector2(columns, rows);
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        }

    }
}
