using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCanvasObject : MonoBehaviour
{

    public Transform pivot;
    public Text text;
    public Animator animator;

    float currentPos = 0.0f;
    bool show = true;
    float showTimer = 0.0f;
    float showLength = 0.5f;
    
    void Awake() {
        GameManager.Instance.dialogueCanvas = this;
        pivot.localPosition = new Vector3(pivot.localPosition.x, -256.0f, pivot.localPosition.z);
    }

    void Update() {
        if (show) {
            showTimer += Time.deltaTime;
            currentPos = Mathf.Lerp(0.0f, 1.0f, Mathf.Clamp01(showTimer / showLength));

            if (showTimer > 5.0f) {
                show = false;
                showTimer = 0.0f;
            }
        } else {
            showTimer += Time.deltaTime;
            currentPos = Mathf.Lerp(1.0f, 0.0f, Mathf.Clamp01(showTimer / showLength));
        }

        if (show) {
            pivot.localPosition = new Vector3(pivot.localPosition.x, Mathf.Lerp(-256,0.0f, currentPos), pivot.localPosition.z);
        } else {
            
            pivot.localPosition = new Vector3(pivot.localPosition.x, Mathf.Lerp(0.0f, -256, currentPos), pivot.localPosition.z);
        }
    }

    public void ShowDialogue() {
        currentPos = 0.0f;
        bool show = true;
        float showTimer = 0.0f;


    }

    public void SetText(string newText) {
        text.text = newText;
    }

    public void SetAnimator(RuntimeAnimatorController anim) {
        //animator.runtimeAnimatorController = anim as RuntimeAnimatorController;
    }

}
