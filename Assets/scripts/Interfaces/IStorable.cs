using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStorable
{
    /// <summary>
    /// Marks a class a storable.
    /// A storable class must be able to quantify its data as a list of floats.
    /// </summary>

    List<float> GetData(); // Returns the current state of the object
    void SetData(List<float> newData, List<float> previousData=null); // Sets the current state of the object
    void ApplyData(float t); // Lerps the object between its previous data and its current data
}