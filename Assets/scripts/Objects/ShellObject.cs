using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActivatableObject))]
public class ShellObject : MonoBehaviour, IInteractable, IStorable
{
    [HideInInspector]
    public ActivatableObject activatableObject;
    public GameObject Shell;
    Renderer shellRenderer;
    bool hasShell = true;

    void Awake() {
        activatableObject = GetComponent<ActivatableObject>();
        shellRenderer = Shell.GetComponent<Renderer>();
    }

    public List<float> GetData() {
        return new List<float>{Shell.activeSelf ? 1.0f : 0.0f};
    }
    public void SetData(List<float> newData, List<float> previousData=null){
        hasShell = newData[0] > 0.5f;
    }
    public void ApplyData(float t){
        Shell.SetActive(hasShell);
    }

    public void OnInteract() {
        if (GameManager.Instance.playerObject.playerObject.hasShell == false) {
            hasShell = false;
            activatableObject.SetActivated(hasShell);
            GameManager.Instance.playerObject.playerObject.hasShell = true;
        }
    }

    public void OnEnterAdjacentCell(GridCell cell) {}

    public void OnLeaveAdjacentCell() {}

}
