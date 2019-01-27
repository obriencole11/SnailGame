using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : Singleton<GameManager>
{

    public SettingsAsset settings;
    public float blendLength = 0.1f;

    bool animating = false;
    float blendValue = 0.0f;

    [HideInInspector]
    public List<BaseObject> sceneObjects = new List<BaseObject>();
    [HideInInspector]
    public List<BaseObject> activatableObjects = new List<BaseObject>();
    [HideInInspector]
    public List<BaseObject> gridObjects = new List<BaseObject>();
    [HideInInspector]
    public List<BaseObject> collisionObjects = new List<BaseObject>();
    [HideInInspector]
    public BaseObject playerObject;
    [HideInInspector]
    public List<BaseObject> interactionObjects = new List<BaseObject>();
    [HideInInspector]
    public List<BaseObject> attachmentObjects = new List<BaseObject>();
    [HideInInspector]
    public List<BaseObject> storableObjects = new List<BaseObject>();

    public void RegisterObject(BaseObject baseObject) {

        sceneObjects.Add(baseObject);
        if (baseObject.hasGridObject) {
            gridObjects.Add(baseObject);
        }
        if (baseObject.isCollidable) {
            collisionObjects.Add(baseObject);
        }
        if (baseObject.isInteractable) {
            interactionObjects.Add(baseObject);
        }
        if (baseObject.isActivatable) {
            activatableObjects.Add(baseObject);
        }
        if (baseObject.isAttachable) {
            attachmentObjects.Add(baseObject);
        }
        if (baseObject.isPlayer) {
            playerObject = baseObject;
        }
        if (baseObject.storableComponents.Length > 0) {
            storableObjects.Add(baseObject);
        }
    }

    void Start() {
        GridManager.Instance.UpdateGridObjects();
        StateManager.Instance.SaveState();
    }

    void Update()
    {   

        // Animate state
        if (animating == true)
        {
            blendValue += Time.deltaTime / blendLength;

            if (blendValue > 1.0f)
            {
                foreach (KeyValuePair<IStorable, List<float>> storableValue in StateManager.Instance.currentState)
                {
                    storableValue.Key.ApplyData(1.0f);
                }
                animating = false;
                blendValue = 0.0f;
            } else {
                foreach (KeyValuePair<IStorable, List<float>> storableValue in StateManager.Instance.currentState)
                {
                    storableValue.Key.ApplyData(blendValue);
                }
            }
        }

        // Handle Input
        bool inputUp = Input.GetButtonDown("Up");
        bool inputDown = Input.GetButtonDown("Down");
        bool inputLeft = Input.GetButtonDown("Left");
        bool inputRight = Input.GetButtonDown("Right");
        bool inputUndo = Input.GetButtonDown("Undo");
        bool inputReset = Input.GetButtonDown("Reset");
        bool input = inputUp || inputDown || inputLeft || inputRight;

        // Handle State
        if (input)
        {
            // Player Movement
            if (inputUp) { playerObject.gridObject.MoveUp(); }
            else if (inputDown) { playerObject.gridObject.MoveDown(); }
            else if (inputLeft) { playerObject.gridObject.MoveLeft(); }
            else if (inputRight) { playerObject.gridObject.MoveRight(); }

            bool sceneChanged = false;

            foreach (BaseObject baseObject in GridManager.Instance.GetObjectsAt(playerObject.gridObject.nextCell)) {
                // Handle Collisions
                if (baseObject.isInteractable) {
                    baseObject.interactionObject.OnInteract();
                    playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
                    sceneChanged = true;
                }
                // Handle Interactions
                else if (baseObject.isCollidable) {
                    playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
                    break;
                }
            }

            // Collide with scene bounds
            if (!GridManager.Instance.CellExists(playerObject.gridObject.nextCell)) {
                playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
            }

            if (playerObject.gridObject.isMoving) {
                SpawnPrefab(settings.slimePrefab, playerObject.gridObject.currentCell);  // Spawn Slimes
                sceneChanged = true;
            }


            if (sceneChanged) {

                // Add input icons
                if (playerObject.gridObject.isMoving) {
                    foreach (BaseObject baseObject in GridManager.Instance.GetAdjacentObjects(playerObject.gridObject.nextCell)){
                        if (baseObject.isInteractable) {
                            baseObject.interactionObject.OnEnterAdjacentCell(playerObject.gridObject.nextCell);
                        }
                    }

                    foreach (BaseObject baseObject in GridManager.Instance.GetAdjacentObjects(playerObject.gridObject.currentCell)){
                        if (baseObject.isInteractable) {
                            baseObject.interactionObject.OnLeaveAdjacentCell();
                        }
                    }
                }

                // Move objects
                playerObject.gridObject.previousCell = playerObject.gridObject.currentCell;
                playerObject.gridObject.currentCell = playerObject.gridObject.nextCell;

                StateManager.Instance.SaveState();
                GridManager.Instance.UpdateGridObjects();
                animating = true;
                blendValue = 0.0f;
            }

        } else if (inputUndo)
        {
            StateManager.Instance.UndoState();
            animating = true;
            blendValue = 0.0f;
            
        } else if (inputReset) {
            StateManager.Instance.ResetState();
            animating = true;
            blendValue = 1.0f;
        }

    }

    GameObject SpawnPrefab(GameObject prefab, GridCell cell) {
        GameObject newObject = Instantiate(prefab) as GameObject;
        newObject.GetComponent<GridObject>().currentCell = cell;
        newObject.GetComponent<GridObject>().previousCell = cell;
        return newObject;
    }

}
