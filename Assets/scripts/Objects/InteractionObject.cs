using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [HideInInspector]
    public GridObject gridObject;
    public SpriteRenderer icon; 

    IInteractable[] interactables;
    

    void Awake()
    {
        gridObject = GetComponent<GridObject>();
        ResetIcon();
        interactables = GetComponents<IInteractable>();
    }

    public void OnInteract()
    {
        foreach (IInteractable interactable in interactables) {
            interactable.OnInteract();
        }
    }

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

    public void OnEnterAdjacentCell(GridCell cell) {
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

    public void OnLeaveAdjacentCell() {
        ResetIcon();
    }
}
