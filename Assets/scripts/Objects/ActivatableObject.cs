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

    public void SetActive(bool _active) {
        active = _active;

        foreach (Transform child in transform) {
            transform.gameObject.SetActive(_active);
        }
    }

    public List<float> GetData()
    {
        return new List<float> { active ? 1.0f : 0.0f };
    }

    public void SetData(List<float> data)
    {
        active = data[0] == 1.0f;
    }

    public void ApplyData(float t)
    {
        gameObject.SetActive(active);
    }
}
