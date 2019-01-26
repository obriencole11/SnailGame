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

        foreach (BaseObject baseObject in GameManager.Instance.sceneObjects)
        {
            foreach (IStorable storable in baseObject.storableComponents)
            {
                newState.Add(storable, storable.GetData());
            }
        }

        // Only update state if it changed
        stateHistory.Add(newState);
    }

    public void LoadState(int index)
    {
        Dictionary<IStorable, List<float>> newState = stateHistory[index];

        foreach (KeyValuePair<IStorable, List<float>> storableState in newState)
        {
            storableState.Key.SetData(storableState.Value);
        }

        stateHistory.Add(newState);
    }

    public void UndoState()
    {

        stateHistory.RemoveAt(currentIndex);
        Dictionary<IStorable, List<float>> newState = stateHistory[currentIndex];

        foreach (KeyValuePair<IStorable, List<float>> storableState in newState)
        {
            storableState.Key.SetData(storableState.Value);
        }
    }

    public void ResetState()
    {
        LoadState(0);
    }
}
