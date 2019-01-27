using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentObject : MonoBehaviour
{

    public GameObject attachPoint;
    GameObject attachedObject;

    void Awake() {
        foreach (Transform transform in attachPoint.transform) {
            attachedObject = transform.gameObject;
        }
    }

    public bool HasAttachedObject() {
        return attachedObject != null;
    }
    
    public void AttachObject(GameObject attachObject) {
        attachedObject = attachObject;
        attachedObject.transform.SetParent(attachPoint.transform, false);
    }

    public GameObject TakeObject() {
        GameObject takenObject = attachedObject;
        attachedObject = null;
        return takenObject;
    } 

}
