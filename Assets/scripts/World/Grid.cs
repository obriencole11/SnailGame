using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int width = 10;
    public int height = 10;

    void Awake()
    {
        GridManager.Instance.grid = this;
    }
}
