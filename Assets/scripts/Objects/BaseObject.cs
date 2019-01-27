using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    
    [HideInInspector]
    public ActivatableObject activatableObject;
    [HideInInspector]
    public GridObject gridObject;
    [HideInInspector]
    public CollisionObject collisionObject;
    [HideInInspector]
    public AttachmentObject attachmentObject;
    [HideInInspector]
    public InteractionObject interactionObject;
    [HideInInspector]
    public PlayerObject playerObject;
    [HideInInspector]
    public IStorable[] storableComponents;

    public bool isActivatable { get { return activatableObject != null; }}
    public bool hasGridObject { get { return gridObject != null; }}
    public bool isCollidable { get { return collisionObject != null; }}
    public bool isInteractable { get { return interactionObject != null; }}
    public bool isPlayer { get { return playerObject != null; }}
    public bool isAttachable { get { return attachmentObject != null; }}

    void Awake()
    {
        activatableObject = GetComponent<ActivatableObject>();
        gridObject = GetComponent<GridObject>();
        collisionObject = GetComponent<CollisionObject>();
        attachmentObject = GetComponent<AttachmentObject>();
        interactionObject = GetComponent<InteractionObject>();
        playerObject = GetComponent<PlayerObject>();
        storableComponents = GetComponents<IStorable>();

        GameManager.Instance.RegisterObject(this);
    }

}
