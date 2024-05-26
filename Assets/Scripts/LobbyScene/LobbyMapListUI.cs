using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMapListUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mapNameText;

    public void Initialize(Lobby lobby)
    {
        if (mapNameText == null)
        {
            Debug.LogError("mapNameText is not assigned.");
            return;
        }

        Debug.Log("Initializing LobbyMapListUI...");

        if (MapSelectionData.Instance == null)
        {
            Debug.LogError("MapSelectionData instance is null. Ensure the MapSelectionData asset is placed in the Resources folder.");
            return;
        }

        if (lobby.Data == null)
        {
            Debug.LogError("Lobby Data is null.");
            mapNameText.text = "Unknown Map";
            return;
        }

        Debug.Log($"Lobby Data: {lobby.Data.Values}");

        if (lobby.Data.ContainsKey("MapIndex"))
        {
            int mapIndex = int.Parse(lobby.Data["MapIndex"].Value);
            Debug.Log($"MapIndex: {mapIndex}");

            if (mapIndex >= 0 && mapIndex < MapSelectionData.Instance.Maps.Count)
            {
                MapInfo mapInfo = MapSelectionData.Instance.Maps[mapIndex];
                mapNameText.text = mapInfo.MapName;
                Debug.Log($"Map Name: {mapInfo.MapName}");
            }
            else
            {
                mapNameText.text = "Unknown Map";
                Debug.LogError("Invalid map index.");
            }
        }
        else
        {
            mapNameText.text = "Unknown Map";
            Debug.LogError("MapIndex not found in lobby data.");
        }


    }

}
