using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableObject : MonoBehaviour, IStorable
{
    [HideInInspector]
    public bool active = true;

    public bool IsActive() {
        return active;
    }

    public void SetActivated(bool _active) {
        active = _active;

        foreach (Transform child in transform) {
            child.gameObject.SetActive(_active);
        }
    }

    public List<float> GetData()
    {
        return new List<float> { active ? 1.0f : 0.0f };
    }

    public void SetData(List<float> data, List<float> previousData=null)
    {
        active = data[0] == 1.0f;

        foreach (Transform child in transform) {
            child.gameObject.SetActive(active);
        }
    }

    public void ApplyData(float t)
    {
        gameObject.SetActive(active);
    }
}
