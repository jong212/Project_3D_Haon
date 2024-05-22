using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMark : MonoBehaviour
{
    public float rotateSpeed = 100.0f;
    public GameObject Mark;
    
   
    void Update()
    {
        Mark.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
