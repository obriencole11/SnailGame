using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public float blendLength = 0.1f;
    bool blending = false;
    float blendValue = 0.0f;

    public List<BaseObject> sceneObjects = new List<BaseObject>();
    public List<GridObject> gridObjects = new List<GridObject>();
    public List<CollisionObject> collisionObjects = new List<CollisionObject>();
    public List<PlayerObject> playerObjects = new List<PlayerObject>();

    void Update()
    {
        /// All Rules Go Here ///
        /// Guidelines:
        /// - Don't Destroy things, instead disable them
        /// 

        if (blending == true)
        {
            blendValue += Time.deltaTime / blendLength;


            if (blendValue > 1.0f)
            {

                foreach (KeyValuePair<IStorable, List<float>> storableValue in StateManager.Instance.currentState)
                {
                    storableValue.Key.LerpData(1.0f);
                }
                blending = false;
                blendValue = 0.0f;
            } else
            {

                foreach (KeyValuePair<IStorable, List<float>> storableValue in StateManager.Instance.currentState)
                {
                    storableValue.Key.LerpData(blendValue);
                }
            }
            
            
        }

        // Handle Input
        bool inputUp = Input.GetKeyDown(KeyCode.W);
        bool inputDown = Input.GetKeyDown(KeyCode.S);
        bool inputLeft = Input.GetKeyDown(KeyCode.A);
        bool inputRight = Input.GetKeyDown(KeyCode.D);
        bool inputUndo = Input.GetKeyDown(KeyCode.Z);
        bool inputReset = Input.GetKeyDown(KeyCode.R);
        bool input = inputUp || inputDown || inputLeft || inputRight;

        // Handle State
        if (input == true)
        {
            // Player Movement
            if (inputUp)
            { foreach (PlayerObject player in playerObjects) { player.gridObject.MoveUp(); } }
            else if (inputDown)
            { foreach (PlayerObject player in playerObjects) { player.gridObject.MoveDown(); } }
            else if (inputLeft)
            { foreach (PlayerObject player in playerObjects) { player.gridObject.MoveLeft(); } }
            else if (inputRight)
            { foreach (PlayerObject player in playerObjects) { player.gridObject.MoveRight(); } }


            // Handle Collisions
            foreach (GridObject gridObject in gridObjects)
            {
                if (gridObject.nextData != gridObject.currentData)
                {
                    foreach (CollisionObject collidable in collisionObjects)
                    {
                        if (collidable.gridObject.currentData[0] == gridObject.nextData[0] && collidable.gridObject.currentData[1] == gridObject.nextData[1])
                        {
                            gridObject.nextData = gridObject.currentData;
                        }
                    }
                }
            }

            // Move objects
            bool sceneChanged = false;
            foreach (PlayerObject player in playerObjects)
            {
                if (player.gridObject.currentData != player.gridObject.nextData)
                {
                    sceneChanged = true;
                }
            }

            if (sceneChanged)
            {

                foreach (GridObject gridObject in gridObjects)
                {
                    gridObject.SetData(gridObject.nextData);
                }

                StateManager.Instance.SaveState();
                blending = true;
                blendValue = 0.0f;
            }

        } else if (inputUndo)
        {
            StateManager.Instance.UndoState();
            blending = true;
            blendValue = 0.0f;
        } else if (inputReset) {
            StateManager.Instance.ResetState();
            blending = true;
            blendValue = 1.0f;
        }

    }
}
