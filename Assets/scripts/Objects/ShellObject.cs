using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellObject : MonoBehaviour, IInteractable
{
    
    BaseObject baseObject;

    void Awake() {
        baseObject = GetComponent<BaseObject>();
    }

    public void OnInteract() {

    }

}
