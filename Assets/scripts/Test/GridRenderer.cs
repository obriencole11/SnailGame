using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridRenderer : MonoBehaviour
{
    private Grid grid;

    void OnDrawGizmos()
    {
        if (grid == null)
        {
            grid = GetComponent<Grid>();
        }

        Gizmos.color = Color.cyan;

        for (int i = 0; i < grid.width + 1; i++) {
            Gizmos.DrawLine(new Vector3(i - 0.5f, 0.0f, -0.5f), new Vector3(i - 0.5f, 0.0f, grid.height - 0.5f));
        }

        for (int i = 0; i < grid.height + 1; i++)
        {
            Gizmos.DrawLine(new Vector3(-0.5f, 0.0f, i - 0.5f), new Vector3(grid.width - 0.5f, 0.0f, i - 0.5f));
        }
    }
}
