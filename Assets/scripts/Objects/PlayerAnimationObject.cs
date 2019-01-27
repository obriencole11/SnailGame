using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationObject : MonoBehaviour, IStorable
{
    [HideInInspector]
    public bool facingRight = true;
    public Transform AnimationPivot;
    public Animator PlayerAnimator;



    public List<float> GetData(){
        return new List<float>{facingRight ? 1.0f : 0.0f};
    }
    public void SetData(List<float> newData, List<float> previousData=null){
        facingRight = newData[0] > 0.5f;
    }
    public void ApplyData(float t) {
        float modifier = facingRight ? 1.0f : -1.0f;
        AnimationPivot.localScale = new Vector3(modifier * Mathf.Abs(AnimationPivot.localScale.x), AnimationPivot.localScale.y, AnimationPivot.localScale.z);
    }
    
    
    public void ApplySquash() {

    }
}
