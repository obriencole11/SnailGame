using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : Singleton<GameManager>
{

    public SettingsAsset settings;
    public float blendLength = 0.1f;

    bool animating = false;
    float blendValue = 0.0f;
    bool acceptInput = true;

    float loadTimer = 0.0f;
    bool loading = false;

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
    }

    public bool isIntro = false;
    public bool isOutro = false;
    public float introTimer = 0.0f;
    public TitleObject titleObject;

    public DialogueCanvasObject dialogueCanvas;


    public void InitializeObjects() {
        foreach (BaseObject baseObject in sceneObjects) {

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
    }

    AudioSource backgroundAudio;
    float fadeInTimer = 0.0f;

    void Awake() {
        settings = Resources.Load<SettingsAsset>("DefaultSettings");

        if (Application.loadedLevel == 0) {
            isIntro = true;
        } 

        GameObject audioObject = Instantiate(settings.AudioSource) as GameObject;
        backgroundAudio = audioObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(audioObject);
    }

    void Start() {

        InitializeScene();
        SceneManager.sceneLoaded += OnLoad;
        
    }

    void InitializeScene() {
        
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
        InitializeObjects();

        GridManager.Instance.UpdateGridObjects();

        StateManager.Instance.ClearState();
        StateManager.Instance.SaveState();
    }

    void OnLoad(Scene scene, LoadSceneMode mode) {

        loading = false;
        loadTimer = 0.0f;

        activatableObjects = new List<BaseObject>();
        gridObjects = new List<BaseObject>();
        collisionObjects = new List<BaseObject>();
        interactionObjects = new List<BaseObject>();
        attachmentObjects = new List<BaseObject>();
        storableObjects = new List<BaseObject>();
        tileObjects = new List<BaseObject>();

        acceptInput = true;

        InitializeScene();
    }

    float ggjStart = 0.25f;
    float ggjFadeOut = 3.0f;
    float titleStart = 4.0f;
    float titleFadeOut = 6.25f;

    float controlTimer = 0.0f;

    void Update()
    {       
        if (fadeInTimer < 1.0f) {
            fadeInTimer += Time.deltaTime / 5.0f;
            backgroundAudio.volume = Mathf.Lerp(0.0f, 0.5f, Mathf.Clamp01(fadeInTimer));
        }

        if (isIntro) {
            introTimer += Time.deltaTime;

            if (introTimer < ggjFadeOut) {
                titleObject.FadeInGGJLogo(introTimer - ggjStart);
            } else if (introTimer >= ggjFadeOut) {
                titleObject.FadeOutGGJLogo(introTimer - ggjFadeOut);
            }

            if (introTimer < titleFadeOut) {
                titleObject.FadeInLogo(introTimer - titleStart);
            } else if (introTimer > titleFadeOut) {
                titleObject.FadeOutLogo(introTimer - titleFadeOut);
            }

            if (introTimer > titleFadeOut + 1.0f) {
                isIntro = false;
            }

            return;
        } else if (Application.loadedLevel == 0 ) {
            controlTimer += Time.deltaTime;
            if (controlTimer > 1.0f) {
                titleObject.FadeInControls(controlTimer - 1.0f);
                titleObject.FadeInUndoControls(controlTimer - 1.0f);
            }
        }


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

        if (loading) {
            loadTimer += Time.deltaTime;
            if (loadTimer > 3.0f) {
                int i = Application.loadedLevel;
                SceneManager.LoadScene(i + 1);
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

        bool getShell = false;

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
                            getShell = true;
                        } else {
                            playerObject.gridObject.nextCell = playerObject.gridObject.currentCell;
                        }
                    } else if (baseObject.isSlug){
                        if (playerObject.playerObject.hasShell == true) {
                            sceneChanged = true;
                            baseObject.activatableObject.active = false;
                            baseObject.slugObject.AddShell();
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
                if (getShell) {
                    playerObject.playerObject.AnimatorShellGet();
                } else {
                    playerObject.playerObject.AnimatorMove();
                }
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
            playerObject.playerObject.AnimatorMove();
            
        } else if (inputReset) {
            StateManager.Instance.ResetState();
            GridManager.Instance.UpdateGridObjects();
            animating = true;
            blendValue = 1.0f;
            playerObject.playerObject.AnimatorMove();
        }

    }

    void LoadNextLevel() {
        
        sceneObjects = new List<BaseObject>();
        loadTimer = 0.0f;
        loading = true;
    }

    public bool LevelComplete() {

        if (playerObject == null) {
            return false;
        }
        
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
