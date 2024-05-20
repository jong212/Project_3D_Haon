using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class TutorialKeyController : MonoBehaviour
{
    public Image AKey;
    public Image SKey;
    public Image DKey;
    public Image WKey;
    public Image ShiftKey;
    public Image MouseLeft;
    public Image Key1;
    public Image Key2;


    
    void Update()
    {
        //transform.LookAt(Camera);
        if (Input.GetKeyDown(KeyCode.A)) AKey.color = Color.red;
        if (Input.GetKeyUp(KeyCode.A)) AKey.color = Color.white;

        if (Input.GetKeyDown(KeyCode.S)) SKey.color = Color.red;
        if (Input.GetKeyUp(KeyCode.S)) SKey.color = Color.white;

        if (Input.GetKeyDown(KeyCode.D)) DKey.color = Color.red;
        if (Input.GetKeyUp(KeyCode.D)) DKey.color = Color.white;

        if (Input.GetKeyDown(KeyCode.W)) WKey.color = Color.red;
        if (Input.GetKeyUp(KeyCode.W)) WKey.color = Color.white;

        if(Input.GetKeyDown(KeyCode.LeftShift)) ShiftKey.color = Color.red;
        if (Input.GetKeyUp(KeyCode.LeftShift)) ShiftKey.color = Color.white;

        if (Input.GetKeyDown(KeyCode.Alpha1)) Key1.color = Color.red;
        if (Input.GetKeyUp(KeyCode.Alpha1)) Key1.color = Color.white;

        if (Input.GetKeyDown(KeyCode.Alpha2)) Key2.color = Color.red;
        if (Input.GetKeyUp(KeyCode.Alpha2)) Key2.color = Color.white;

        if(Input.GetMouseButtonDown(0)) MouseLeft.color = Color.red;
        if (Input.GetMouseButtonUp(0)) MouseLeft.color = Color.white;
    }

    


}
