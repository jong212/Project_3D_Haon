using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/MapSelectionData", fileName ="MapSelectionData")]
public class MapSelectionData : ScriptableObject
{
    private static MapSelectionData _instance;
    public static MapSelectionData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<MapSelectionData>("MapSelectionData");
                if (_instance == null)
                {
                    Debug.LogError("Failed to load MapSelectionData from Resources.");
                }
            }
            return _instance;
        }
    }

    public List<MapInfo> Maps;
}

[Serializable]
public struct MapInfo
{
    // 이미지로 변경
    public Sprite MapImage;
    public Color MapThumbnail;
    public string MapName;
    public string SceneName;
}

