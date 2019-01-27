using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseObject))]
[RequireComponent(typeof(GridObject))]
public class SlimeSpriteObject : MonoBehaviour, IStorable
{
    
    public SpriteRenderer sprite;
    public Sprite dotSprite;
    public Sprite lineSprite;
    public Sprite lineCapSprite;
    public Sprite cornerSprite;
    [HideInInspector]
    public GridObject gridObject;

    GridCell nextCell;
    GridCell previousCell;

    public void SetNextCell(GridCell cell) {
        if (GridManager.Instance.CellExists(cell)) {
            nextCell = cell;
        }
    }

    public void SetPreviousCell(GridCell cell) {
        if (GridManager.Instance.CellExists(cell)) {
            previousCell = cell;
        }
    }

    void Awake() {
        gridObject = GetComponent<GridObject>();

        nextCell = gridObject.currentCell;
        previousCell = gridObject.currentCell;
    }

    public List<float> GetData(){
        return new List<float>{
            (float)nextCell.x,
            (float)nextCell.y,
            (float)previousCell.x,
            (float)previousCell.y
        };
    }

    public void SetData(List<float> newData, List<float> previousData=null){
        nextCell = new GridCell((int)newData[0], (int)newData[1]);
        previousCell = new GridCell((int)newData[2], (int)newData[3]);
    }

    public void ApplyData(float t){
        UpdateSprite();
    }

    void UpdateSprite() {
        
        // Reset orientation
        transform.localRotation = Quaternion.identity;

        Sprite newSprite;
        bool previousCellExists = previousCell != null && previousCell != gridObject.currentCell;
        bool nextCellExists = nextCell != null && nextCell != gridObject.currentCell;

        if (!nextCellExists && !previousCellExists) {
            // Cell is center cell
            newSprite = dotSprite;
        } else if (nextCellExists && !previousCellExists) {
            // End point facing next cell
            newSprite = GetEndCapSprite(nextCell);
        } else if (previousCellExists && !nextCellExists) {
            // End point facing next cell
            newSprite = GetEndCapSprite(previousCell);
        } else if (previousCell.x == nextCell.x){
            // Horizontal Line
            newSprite = lineSprite;
        } else if (previousCell.y == nextCell.y) {
            // Vertical Line
            newSprite = lineSprite;
            transform.localRotation = Quaternion.Euler(0,90,0);
        } else {
            // Determine type of corner sprite
            newSprite = GetCornerSprite();
        }

        sprite.sprite = newSprite;
    }

    Sprite GetEndCapSprite(GridCell destination) {
        if (destination.y > gridObject.currentCell.y) {
            transform.localRotation = Quaternion.Euler(0,180,0);
        } else if (destination.x < gridObject.currentCell.x) {
            transform.localRotation = Quaternion.Euler(0,90,0);
        } else if (destination.x > gridObject.currentCell.x) {
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        
        return lineCapSprite;
    }

    Sprite GetCornerSprite() {

        if (nextCell.x > gridObject.currentCell.x || previousCell.x > gridObject.currentCell.x) {

            if (nextCell.y > gridObject.currentCell.y || previousCell.y > gridObject.currentCell.y) {
                // Top and Right
                transform.localRotation = Quaternion.Euler(0, -90, 0);
            }

        } else {
            if (nextCell.y > gridObject.currentCell.y || previousCell.y > gridObject.currentCell.y) {
                // Top and Left
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            } else {
                // Bottom and Left
                transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
        }

        return cornerSprite;
    }

}
