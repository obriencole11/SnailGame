using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    void Awake()
    {
        base.Awake();
        GameManager.Instance.playerObjects.Add(this);
    }
}
