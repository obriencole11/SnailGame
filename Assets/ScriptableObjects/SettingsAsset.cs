using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/SettingsAsset", order = 1)]
public class SettingsAsset : ScriptableObject
{
    public GameObject  slimePrefab;
    public Sprite upArrow;
    public Sprite downArrow;
    public Sprite leftArrow;
    public Sprite rightArrow;
}
