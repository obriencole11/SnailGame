using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionIconObject : MonoBehaviour, IInteractable
{
    [HideInInspector]
    public GridObject gridObject;
    public SpriteRenderer icon; 
    

    void Awake()
    {
        gridObject = GetComponent<GridObject>();
        ResetIcon();
    }

    void Update() {
        GridCell cell = GameManager.Instance.playerObject.gridObject.currentCell;
        if (cell.left == gridObject.currentCell) {
            SetIcon(GameManager.Instance.settings.leftArrow);
        } else if (cell.right == gridObject.currentCell) {
            SetIcon(GameManager.Instance.settings.rightArrow);
        } else if (cell.up == gridObject.currentCell) {
            SetIcon(GameManager.Instance.settings.upArrow);
        } else if (cell.down == gridObject.currentCell){
            SetIcon(GameManager.Instance.settings.downArrow);
        } else {
            ResetIcon();
        }
    }

    public void OnInteract() {}

    void SetIcon(Sprite sprite) {
        if (icon == null) {
            return;
        }
        icon.enabled = true;

        if (sprite != null) {
            icon.sprite = sprite;
        }
    }

    void ResetIcon() {
        if (icon == null) {
            return;
        }
        icon.enabled = false;
    }
}
