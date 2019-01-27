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
    public PlayerObject playerObject;
    [HideInInspector]
    public TileObject tileObject;
    [HideInInspector]
    public SlugObject slugObject;
    [HideInInspector]
    public ShellObject shellObject;
    [HideInInspector]
    public ExitObject exitObject;
    [HideInInspector]
    public IStorable[] storableComponents;
    [HideInInspector]
    public IInteractable[] interactionComponents;

    public bool isActivatable { get { return activatableObject != null; }}
    public bool hasGridObject { get { return gridObject != null; }}
    public bool isCollidable { get { return collisionObject != null; }}
    public bool isInteractable { get { return interactionComponents != null; }}
    public bool isPlayer { get { return playerObject != null; }}
    public bool isTile { get { return tileObject != null; }}
    public bool isSlug { get { return slugObject != null; }}
    public bool isShell { get { return shellObject != null; }}
    public bool isExit { get { return exitObject != null; }}
    
    void Awake()
    {

        GameManager.Instance.RegisterObject(this);

        activatableObject = GetComponent<ActivatableObject>();
        gridObject = GetComponent<GridObject>();
        collisionObject = GetComponent<CollisionObject>();
        tileObject = GetComponent<TileObject>();
        exitObject = GetComponent<ExitObject>();
        slugObject = GetComponent<SlugObject>();
        shellObject = GetComponent<ShellObject>();
        interactionComponents = GetComponents<IInteractable>();
        playerObject = GetComponent<PlayerObject>();
        storableComponents = GetComponents<IStorable>();
    }

}
