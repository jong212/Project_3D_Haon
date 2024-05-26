using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/MapSelectionData", fileName ="MapSelectionData")]
public class MapSelectionData : ScriptableObject
{
    public static MapSelectionData Instance;

    public List<MapInfo> Maps;

    private void OnEnable()
    {
        Instance = this;
    }
}

[Serializable]
public struct MapInfo
{
    public Sprite MapImage;
    public string MapName;
    public string SceneName;
}
