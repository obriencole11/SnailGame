using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridCell
{
    public int x;
    public int y;

    public GridCell (int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public GridCell left { get { return new GridCell(x - 1, y); }}
    public GridCell right { get { return new GridCell(x + 1, y); }}
    public GridCell up { get { return new GridCell(x, y + 1); }}
    public GridCell down { get { return new GridCell(x, y - 1); }}

    public bool isAdjacentTo(GridCell otherCell) {
        bool adjacent = false;

        if (Mathf.Abs(otherCell.x - x) == 1) {
            adjacent = true;
        } else if (Mathf.Abs(otherCell.y - y) == 1) {
            adjacent = true;
        }

        return adjacent;
    }

    public static GridCell FromPosition (Vector3 position) {
        return new GridCell((int)Mathf.Round(position.x), (int)Mathf.Round(position.y));
    }

    public Vector3 ToPosition() {
        return new Vector3((float)x, 0.0f, (float)y);
    }

    public static bool operator == (GridCell a, GridCell b)
    {
        // an item is always equal to itself
        if (object.ReferenceEquals(a, b))
            return true;

        // if both a and b were null, we would have already escaped so check if either is null
        if (object.ReferenceEquals(a, null))
            return false;
        if (object.ReferenceEquals(b, null))
            return false;

        // Now that we've made sure we are working with real objects:
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(GridCell a, GridCell b)
    {
        return !(a == b);
    }

    public override bool Equals(System.Object obj)
    {
        return (obj as GridCell) == this;
    }

    public override string ToString()
    {
        return System.String.Format("GridCell {0}, {1}", x, y);;
    }
}

public class GridManager : Singleton<GridManager>
{
    [HideInInspector]
    public Grid grid;
    public List<BaseObject>[,] gridContents;

    public List<BaseObject> GetObjectsAt (GridCell cell) {
        if (CellExists(cell)) {
            return gridContents[cell.x, cell.y];
        } else {
            return new List<BaseObject>();
        }
    }

    public bool CellExists(GridCell cell) {
        if (cell.x < 0) {
            return false;
        }
        if (cell.y < 0) {
            return false;
        }
        if (cell.x >= grid.width) {
            return false;
        }
        if (cell.y >= grid.height) {
            return false;
        }
        return true;
    }

    public List<BaseObject> GetAdjacentObjects (GridCell cell) {
        List<BaseObject> adjacentObjects = new List<BaseObject>();

        foreach (GridCell adjacentCell in new List<GridCell>{cell.left, cell.right, cell.up, cell.down}) {
            if (CellExists(adjacentCell)) {
                foreach (BaseObject baseObject in GetObjectsAt(adjacentCell)) {
                    adjacentObjects.Add(baseObject);
                }
            }
        }
        return adjacentObjects;
    }

    public void UpdateGridObjects() {
        // Updates the grid contents list with all active grid objects
        gridContents = new List<BaseObject>[grid.width, grid.height];

        for (int x = 0; x < grid.width; x++) {
            for (int y = 0; y < grid.height; y++) {
                gridContents[x, y] = new List<BaseObject>();
            }
        }

        foreach (BaseObject baseObject in GameManager.Instance.gridObjects) {

            // Only grab objects on grid that are activatable
            if (baseObject.isActivatable && baseObject.hasGridObject) {
                // Only grab active objects
                if (baseObject.activatableObject.IsActive()) {

                    // Add them to the dictionary with their cell as the key
                    gridContents[baseObject.gridObject.currentCell.x, baseObject.gridObject.currentCell.y].Add(baseObject);
                }
            }
        }
    }

}
