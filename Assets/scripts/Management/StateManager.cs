using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : Singleton<StateManager>
{

    public List<Dictionary<IStorable, List<float>>> stateHistory = new List<Dictionary<IStorable, List<float>>>();
    public int currentIndex { get { return stateHistory.Count - 1; } }
    public int previousIndex { get { return stateHistory.Count - 2; } }
    public Dictionary<IStorable, List<float>> currentState { get { return stateHistory[currentIndex]; } }

    public void SaveState()
    {
        Dictionary<IStorable, List<float>> newState = new Dictionary<IStorable, List<float>>();

        foreach (BaseObject baseObject in GameManager.Instance.storableObjects)
        {
            foreach (IStorable storable in baseObject.storableComponents)
            {
                newState.Add(storable, storable.GetData());
            }
        }

        stateHistory.Add(newState);
    }

    public void ClearState() {
        stateHistory = new List<Dictionary<IStorable, List<float>>>();
    }

    public void LoadState(int index)
    {
        Dictionary<IStorable, List<float>> newState = stateHistory[index];

        foreach (KeyValuePair<IStorable, List<float>> storableState in newState)
        {
            storableState.Key.SetData(storableState.Value);
        }

        foreach (BaseObject baseObject in GameManager.Instance.sceneObjects) {
            bool inHistory = false;
            foreach (IStorable storable in baseObject.GetComponents<IStorable>()) {
                if (newState.ContainsKey(storable)) {
                    inHistory = true;
                    break;
                }
            }

            if (!inHistory) {
                baseObject.activatableObject.SetActivated(false);
            }
        }

        stateHistory.Add(newState);
    }

    public void UndoState()
    {
        if (stateHistory.Count > 1) {

            stateHistory.RemoveAt(currentIndex);

            Dictionary<IStorable, List<float>> newState = stateHistory[currentIndex];

            foreach (KeyValuePair<IStorable, List<float>> storableState in newState)
            {
                storableState.Key.SetData(storableState.Value);
            }

            foreach (BaseObject baseObject in GameManager.Instance.sceneObjects) {
                bool inHistory = false;
                foreach (IStorable storable in baseObject.GetComponents<IStorable>()) {
                    if (newState.ContainsKey(storable)) {
                        inHistory = true;
                        break;
                    }
                }

                if (!inHistory) {
                    baseObject.activatableObject.SetActivated(false);
                }
            }
        }
    }

    public void ResetState()
    {
        LoadState(0);
    }
}
