using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseObject))]
[RequireComponent(typeof(GridObject))]
[RequireComponent(typeof(AttachmentObject))]
public class PlayerObject : MonoBehaviour
{

    [HideInInspector]
    public GridObject gridObject;
    [HideInInspector]
    public BaseObject baseObject;

    [HideInInspector]
    public AttachmentObject attachmentObject;
    public bool hasShell = false;
    
    protected void Awake()
    {
        baseObject = GetComponent<BaseObject>();
        gridObject = GetComponent<GridObject>();
        attachmentObject = GetComponent<AttachmentObject>();
    }

    public void AddShell() {

    }

    public void RemoveShell() {

    }
}
