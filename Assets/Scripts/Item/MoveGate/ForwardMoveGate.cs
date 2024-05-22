using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ForwardMoveGate : MonoBehaviour
{
    public GameObject tile;
    public GameObject secretWall;
    public float speed = 1f;
    public bool back = false;
    public float stopTime= 0;

    private bool isMove = false;
    private int point = 0;

    private void Update()
    {
        ForwardBackMoveGate();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isMove = true;
        }
    }

    void ForwardBackMoveGate()
    {
        if (isMove && point == 0 && back == false)
        {
            secretWall.SetActive(false);
            tile.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            Invoke("MoveRimit", stopTime);
        }
        else if (isMove && point == 0 && back == true)
        {
            secretWall.SetActive(false);
            tile.transform.Translate(Vector3.back* speed * Time.deltaTime);
            Invoke("MoveRimit", stopTime);
        }
    }

    void MoveRimit()
    { 
        isMove = false;
        point++;
    }
}
