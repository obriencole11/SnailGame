using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour, IStorable
{
    /// <summary>
    /// The base of all puzzle gameObjects.
    /// This stores active/inactive values 
    /// as well as serving as a nexus for other components.
    /// </summary>

    public bool active = true;
    public List<Component> components;
    public List<IStorable> storableComponents;

    void Awake()
    {
        GameManager.Instance.sceneObjects.Add(this);
        storableComponents = new List<IStorable>(GetComponents<IStorable>());
    }

    public List<float> GetData()
    {
        return new List<float> { active ? 1.0f : 0.0f };
    }

    public void SetData(List<float> data)
    {
        active = data[0] == 1.0f;
    }

    public void LerpData(float t)
    {
        return;
    }

}
