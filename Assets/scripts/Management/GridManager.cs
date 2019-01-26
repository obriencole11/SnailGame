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
}

public class GridManager : Singleton<GridManager>
{
    public List<GridObject> gridObjects = new List<GridObject>();
    public Grid grid { get; set; }

    public List<GridObject> GetGridObjects<T>()
    {
        List<GridObject> returnObjects = new List<GridObject>();

        foreach (GridObject gridObject in gridObjects)
        {
            if (gridObject.gameObject.GetComponent(typeof(T)) != null)
            {
                returnObjects.Add(gridObject);
            }
        }

        return returnObjects;
    }
}
