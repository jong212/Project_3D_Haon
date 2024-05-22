using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/MapSelectionData", fileName ="MapSelectionData")]
public class MapSelectionData : ScriptableObject
{
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

