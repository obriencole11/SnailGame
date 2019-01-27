using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : Singleton<GameManager>
{

    public SettingsAsset settings;
    public float blendLength = 0.1f;

    bool animating = false;
    float blendValue = 0.0f;
    bool acceptInput = true;

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
    [HideInInspector]
    public List<BaseObject> tileObjects = new List<BaseObject>();

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
        if (baseObject.isTile) {
            tileObjects.Add(baseObject);
        }
        if (baseObject.isPlayer) {
            playerObject = baseObject;
        }
        if (baseObject.storableComponents.Length > 0) {
            storableObjects.Add(baseObject);
        }
    }

    void Start() {

        // Generate a slime for every square
        GameObject slimeParent = new GameObject();
        slimeParent.name = "Slimes";
        for (int x = 0; x < GridManager.Instance.grid.width; x ++) {
            for (int y = 0; y < GridManager.Instance.grid.height; y ++) {
                GameObject slime = SpawnPrefab(settings.slimePrefab, new GridCell(x, y));
                slime.GetComponent<ActivatableObject>().SetActivated(false);
                slime.transform.SetParent(slimeParent.transform);
            }
        }

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
        if (!acceptInput) {return;}
        else if (input)
        {
            // Player Movement
            if (inputUp) { playerObject.gridObject.MoveUp(); }
            else if (inputDown) { playerObject.gridObject.MoveDown(); }
            else if (inputLeft) { playerObject.gridObject.MoveLeft(); }
            else if (inputRight) { playerObject.gridObject.MoveRight(); }

            bool sceneChanged = false;

            bool containsTile = false;
            foreach (BaseObject baseObject in GridManager.Instance.GetObjectsAt(playerObject.gridObject.nextCell)) {

                if (baseObject.isTile == true) {
                    containsTile = true;
                    continue;
                }
                // Handle Interactions
                if (baseObject.isInteractable && baseObject.activatableObject.IsActive()) {

                    if (baseObject.isShell) {
                        if (playerObject.playerObject.hasShell == false) {
                            sceneChanged = true;
                        } else {
                            playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
                        }
                    } else if (baseObject.isSlug){
                        if (playerObject.playerObject.hasShell == true) {
                            sceneChanged = true;
                            baseObject.activatableObject.SetActivated(false);
                            playerObject.playerObject.hasShell = false;
                            playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
                        } else {
                            playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
                        }
                    
                    } else if (baseObject.isExit){
                        if (LevelComplete()) {
                            sceneChanged = true;
                            acceptInput = false;
                            LoadNextLevel();
                        } else {
                            playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
                            sceneChanged = false;
                        }
                    
                    } else {
                        playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
                    }

                    foreach(IInteractable interactable in baseObject.interactionComponents){
                        interactable.OnInteract();
                    }
                }
                // Handle Collisions
                else if (baseObject.isCollidable && baseObject.activatableObject.IsActive()) {
                    playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
                }
            }

            // Collide with scene bounds
            if (!GridManager.Instance.CellExists(playerObject.gridObject.nextCell)) {
                playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
            }

            // Collide with non-tiles
            if (containsTile == false) {
                playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
            }

            // Spawn Slimes
            GenerateSlimes();

            if (playerObject.gridObject.nextCell.x < playerObject.gridObject.currentCell.x) {
                playerObject.GetComponent<PlayerAnimationObject>().facingRight = false;
            } else if (playerObject.gridObject.nextCell.x > playerObject.gridObject.currentCell.x) {
                playerObject.GetComponent<PlayerAnimationObject>().facingRight = true;
            }

            if (playerObject.gridObject.isMoving) {
                sceneChanged = true;
            }


            if (sceneChanged) {

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
            GridManager.Instance.UpdateGridObjects();
            animating = true;
            blendValue = 0.0f;
            
        } else if (inputReset) {
            StateManager.Instance.ResetState();
            GridManager.Instance.UpdateGridObjects();
            animating = true;
            blendValue = 1.0f;
        }

    }

    void LoadNextLevel() {

    }

    public bool LevelComplete() {

        
        foreach (BaseObject baseObject in sceneObjects) {
            if (baseObject.activatableObject.IsActive()) {
                if (baseObject.isShell) {
                    return false;
                }
                if (baseObject.isSlug) {
                    return false;
                }
            }
        }

        foreach (BaseObject baseObject in GridManager.Instance.GetObjectsAt(playerObject.gridObject.nextCell)) {
            if (baseObject.isExit) {
                return true;
            }
        }

        return false;
    }

    void GenerateSlimes() {

        if (playerObject.gridObject.isMoving) {
            
            
            foreach (BaseObject baseObject in GridManager.Instance.GetObjectsAt(playerObject.gridObject.previousCell)) {
                if (baseObject.GetComponent<SlimeSpriteObject>() != null) {
                    baseObject.activatableObject.SetActivated(true);
                    baseObject.GetComponent<SlimeSpriteObject>().SetNextCell(playerObject.gridObject.currentCell);
                }
            }

            foreach (BaseObject baseObject in GridManager.Instance.GetObjectsAt(playerObject.gridObject.currentCell)) {
                if (baseObject.GetComponent<SlimeSpriteObject>() != null) {
                    baseObject.activatableObject.SetActivated(true);
                    baseObject.GetComponent<SlimeSpriteObject>().SetNextCell(playerObject.gridObject.currentCell);
                    baseObject.GetComponent<SlimeSpriteObject>().SetPreviousCell(playerObject.gridObject.previousCell);
                }
            }
        }
    }

    GameObject SpawnPrefab(GameObject prefab, GridCell cell) {
        GameObject newObject = Instantiate(prefab) as GameObject;
        newObject.transform.position = cell.ToPosition();
        newObject.GetComponent<GridObject>().nextCell = cell;
        newObject.GetComponent<GridObject>().currentCell = cell;
        newObject.GetComponent<GridObject>().previousCell = cell;
        return newObject;
    }

}
