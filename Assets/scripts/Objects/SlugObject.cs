using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugObject : MonoBehaviour, IInteractable, IStorable
{
    

    public GameObject shellSprite;
    public RuntimeAnimatorController animator;
    
    private Animator shellAnimator;

    bool hasShell = false;
    
    
    public System.String beforeDialogue = "Hey man I need a shell over here.";
    public System.String afterDialogue = "Hey man I need a shell over here.";

    void Awake() {
        shellAnimator = GetComponent<Animator>();
    }

    public void AddShell() {
        shellSprite.SetActive(true);
        hasShell = true;
        shellAnimator.SetTrigger("GetShell");
    }
    
    public List<float> GetData(){
        return new List<float>{hasShell ? 1.0f : 0.0f};
    }
    public void SetData(List<float> newData, List<float> previousData=null){
        hasShell = newData[0] > 0.5f;
    }
    public void ApplyData(float t){
        shellSprite.SetActive(hasShell);
    }


    public void OnInteract(){

        if (GameManager.Instance.dialogueCanvas == null) {
            return;
        }

        if (shellSprite.active == true) {
            GameManager.Instance.dialogueCanvas.SetText(afterDialogue);
            GameManager.Instance.dialogueCanvas.SetAnimator(animator);
            GameManager.Instance.dialogueCanvas.ShowDialogue();

        } else {
            
            GameManager.Instance.dialogueCanvas.SetText(beforeDialogue);
            GameManager.Instance.dialogueCanvas.SetAnimator(animator);
            GameManager.Instance.dialogueCanvas.ShowDialogue();
        }


    }   
}
