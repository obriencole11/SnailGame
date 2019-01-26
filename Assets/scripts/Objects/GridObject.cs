using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseObject))]
public class GridObject : MonoBehaviour, IStorable
{
    public GridCell position { get { return new GridCell(x, y);  } }
    public int x
    {
        get
        {
            return (int)Mathf.Round(transform.position.x);
        }
    }

    public int y
    {
        get
        {
            return (int)Mathf.Round(transform.position.z);
        }
    }

    public List<float> previousData;
    public List<float> currentData;
    public List<float> nextData;
    public BaseObject baseObject;


    void Awake()
    {
        currentData = new List<float> { x, y };
        previousData = new List<float> { x, y };
        nextData = new List<float> { x, y };

        baseObject = GetComponent<BaseObject>();
        GameManager.Instance.gridObjects.Add(this);
    }

    public void MoveUp()
    {
        nextData = new List<float> { currentData[0], currentData[1] + 1 };
    }

    public void MoveDown()
    {
        nextData = new List<float> { currentData[0], currentData[1] - 1 };
    }

    public void MoveLeft()
    {
        nextData = new List<float> { currentData[0] - 1, currentData[1] };
    }

    public void MoveRight()
    {
        nextData = new List<float> { currentData[0] + 1, currentData[1] };
    }

    public List<float> GetData()
    {
        return currentData;
    }

    public void SetData(List<float> newData)
    {
        previousData = currentData;
        currentData = newData;
    }

    public void LerpData(float t)
    {
        List<float> newData = new List<float>
        {
            Mathf.Lerp(previousData[0], currentData[0], t),
            Mathf.Lerp(previousData[1], currentData[1], t),
        };

        transform.position = new Vector3(newData[0], transform.position.y, newData[1]);
    }

}
