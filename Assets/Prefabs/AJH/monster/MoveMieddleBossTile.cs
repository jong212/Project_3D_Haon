using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMieddleBossTile : MonoBehaviour
{
    public MieddleBossDieCheck mieddleBoss;  
    public GameObject destoryWall;
    public float moveSpeed = 1;
    public float stopTime = 4.5f;

    private bool stopMove = false;

    private void Update()
    {
        OnMoveTile();
    }

    void OnMoveTile()
    {
        if (mieddleBoss.dieCheck == true && !stopMove)
        { 
          transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
          Invoke("MoveStop", stopTime);
        }
    }

    void MoveStop()
    {
        stopMove = true;
        destoryWall.SetActive(false);
    }
}
