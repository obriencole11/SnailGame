using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseObject))]
[RequireComponent(typeof(GridObject))]
public class PlayerObject : MonoBehaviour, IStorable
{

    [HideInInspector]
    public GridObject gridObject;
    [HideInInspector]
    public BaseObject baseObject;
    public SpriteRenderer shellSprite;

    public bool hasShell = false;
    
    protected void Awake()
    {
        baseObject = GetComponent<BaseObject>();
        gridObject = GetComponent<GridObject>();
    }

    public List<float> GetData(){
        return new List<float>{hasShell ? 1.0f : 0.0f};
    }
    public void SetData(List<float> newData, List<float> previousData=null){
        hasShell = newData[0] > 0.5f;
    }
    public void ApplyData(float t){
        // shellSprite.active = hasShell;
    }
}
